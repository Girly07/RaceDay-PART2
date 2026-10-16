using System;

namespace RaceDay.API.Models
{
    public class Result
    {
        public int ResultID { get; set; }
        public int EnrolmentID { get; set; }
        public TimeSpan FinishTime { get; set; }
        public int Position { get; set; }
        public string Status { get; set; } = "Finished";

        // Navigation Property
        public Enrolment? Enrolment { get; set; }
    }
}