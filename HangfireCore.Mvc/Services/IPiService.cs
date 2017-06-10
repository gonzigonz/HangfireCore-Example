using System.Collections.Generic;
using HangfireCore.Mvc.Models;

namespace HangfireCore.Mvc.Services
{
    public interface IPiService
    {
        IEnumerable<PiJob> GetJobs();
        PiJob CreateJob(int digits, int iterations);
        void ScheduleJob(PiJob job);
    }
}