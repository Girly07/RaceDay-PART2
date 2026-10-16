namespace RaceDay.API.Model
{
    public class Result
    {
        public int ResultID { get; set; }
        public int EnrolmentID { get; set; }
        public TimeSpan FinishTime { get; set; }
        public int Position { get; set; }
        public string Status { get; set; } = "Finished";
    }
}
