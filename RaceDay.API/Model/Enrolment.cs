namespace RaceDay.API.Model
{
    public class Enrolment
    {
        public int EnrolmentID { get; set; }
        public int ParticipantID { get; set; }
        public int EventCategoryID { get; set; }
        public DateTime EnrolmentDate { get; set; } = DateTime.Now;
        public string Status { get; set; } = "Pending";
        public string PaymentStatus { get; set; } = "Unpaid";
    }
}
