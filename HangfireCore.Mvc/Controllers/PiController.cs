using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HangfireCore.Mvc.Services;
using HangfireCore.Mvc.Models;
using HangfireCore.Mvc.Data;

namespace HangfireCore.Mvc.Controllers
{
    [Route("api/[controller]/[action]")]
    public class PiController : Controller
    {
        private readonly AppDbContext _ctx;
        private readonly IMathService _mathService;

        public PiController(
            AppDbContext ctx,
            IMathService mathService)
        {
            _ctx = ctx;
            _mathService = mathService;
        }

        [HttpGet]
        public IActionResult List()
        {
            var list = _ctx.PiJobs;
            return Ok(list);
        }
        
        [HttpPost]
        public IActionResult QueueJob(int digits, int iterations)
        {
            if(digits < 1 || iterations < 1){
                return BadRequest("Values required!");
            }

            var piJob = new PiJob()
            {
                Digits = digits,
                Iterations = iterations,
                Status = "Processing",
                StartTime = DateTime.Now
            };

            _ctx.PiJobs.AddAsync(piJob);
            _ctx.SaveChangesAsync();

            var result = _mathService.GetPi(piJob.Digits, piJob.Iterations);

            piJob.Result = result.ToString();
            piJob.EndTime = DateTime.Now;
            piJob.Status = "Complete";
            _ctx.PiJobs.Update(piJob);
            _ctx.SaveChangesAsync();

            return Ok();
        }
    }
}