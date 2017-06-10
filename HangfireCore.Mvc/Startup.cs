using System;
using System.Diagnostics;
using Hangfire;
using Hangfire.MemoryStorage;
using HangfireCore.Mvc.Data;
using HangfireCore.Mvc.Infrastructure;
using HangfireCore.Mvc.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace HangfireCore.Mvc
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddDbContext<AppDbContext>(opts => opts.UseSqlite("DataSource=HangfireCore.db"));
            services.AddHangfire( opts => opts.UseMemoryStorage());
            services.AddMvc();

            // Register other services.
            services.AddScoped<IMathService, MathService>();
            services.AddScoped<IPiService, PiService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app, 
            IHostingEnvironment env, 
            ILoggerFactory loggerFactory,
            IServiceProvider serviceProvider,
            AppDbContext ctx)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            ctx.Database.EnsureDeleted();
            ctx.Database.EnsureCreated();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            GlobalConfiguration.Configuration.UseActivator(new HangfireActivator(serviceProvider));
            app.UseHangfireServer();
            app.UseHangfireDashboard();
        }
    }
}
