using System;
using System.Numerics;

namespace HangfireCore.Mvc.Models
{
    public class PiJob
    {
        public int Id { get; set; } 
        public int Digits { get; set; }
        public int Iterations { get; set; }
        public string Result { get; set; }
        public string Status { get; set; }
        public DateTime StartTime { get; set; } 
        public DateTime EndTime { get; set; }   
        
    }
}