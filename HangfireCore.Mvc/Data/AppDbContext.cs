using System;
using HangfireCore.Mvc.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HangfireCore.Mvc.Data
{

  public class AppDbContext : DbContext
  {
    public AppDbContext(DbContextOptions<AppDbContext> options)
      : base(options)
    {
    }

    public DbSet<PiJob> PiJobs { get; set; }
  }
}