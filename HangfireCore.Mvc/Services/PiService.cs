using System;
using System.Collections.Generic;
using HangfireCore.Mvc.Data;
using HangfireCore.Mvc.Models;

namespace HangfireCore.Mvc.Services
{
    public class PiService : IPiService
    {
        private readonly AppDbContext _ctx;
        private readonly IMathService _mathService;
        
        public PiService(
            AppDbContext ctx,
            IMathService mathService)
        {
            _ctx = ctx;
            _mathService = mathService;

        }
        PiJob IPiService.CreateJob(int digits, int iterations)
        {
            var piJob = new PiJob()
            {
                Digits = digits,
                Iterations = iterations,
                Status = "Processing",
                StartTime = DateTime.Now
            };

            _ctx.PiJobs.AddAsync(piJob);
            _ctx.SaveChangesAsync();

            return piJob;
        }

        IEnumerable<PiJob> IPiService.GetJobs()
        {
            var list = _ctx.PiJobs;
            return list;
        }

        void IPiService.ScheduleJob(PiJob job)
        {
            var result = _mathService.GetPi(job.Digits, job.Iterations);

            job.Result = result.ToString();
            job.EndTime = DateTime.Now;
            job.Status = "Complete";
            
            _ctx.PiJobs.Update(job);
            _ctx.SaveChangesAsync();
        }
    }
}