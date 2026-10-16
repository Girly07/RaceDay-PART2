using System;

namespace RaceDay.API.Models
{
    public class Enrolment
    {
        public int EnrolmentID { get; set; }
        public int ParticipantID { get; set; }
        public int EventCategoryID { get; set; }
        public DateTime EnrolmentDate { get; set; } = DateTime.Now;
        public string Status { get; set; } = "Pending";
        public string PaymentStatus { get; set; } = "Unpaid";

        // This line fixes the "EventCategory" error in your Enrolments controller:
        public EventCategory? EventCategory { get; set; }
    }
}