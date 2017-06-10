using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HangfireCore.Mvc.Services;
using HangfireCore.Mvc.Models;
using HangfireCore.Mvc.Data;
using Hangfire;

namespace HangfireCore.Mvc.Controllers
{
    [Route("api/[controller]/[action]")]
    public class PiController : Controller
    {
        private readonly IPiService _piService;

        public PiController(IPiService piService)
        {
            _piService = piService;
        }

        [HttpGet]
        public IActionResult List()
        {
            return Ok(_piService.GetJobs());
        }
        
        [HttpPost]
        public IActionResult QueueJob(int digits, int iterations)
        {
            // Validation
            if(digits < 1 || iterations < 1){
                return BadRequest("Values required!");
            }

            if(digits > 32){
                return BadRequest("Digits can not be greater than 32!");
            }

            if(iterations > 100000){
                return BadRequest("Iterations can not be greater than 100,000!");
            }

            // Create job
            var job = _piService.CreateJob(digits, iterations);

            // Schedule job
            var backgroundJobId = BackgroundJob.Enqueue<IPiService>( ps => ps.ScheduleJob(job));
            
            // Done
            return Ok(new { backgroundJobId });
        }
    }
}