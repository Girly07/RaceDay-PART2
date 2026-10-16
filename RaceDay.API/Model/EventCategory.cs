namespace RaceDay.API.Model
{
    public class EventCategory
    {
        public int EventCategoryID { get; set; }
        public int EventID { get; set; }
        public int CategoryID { get; set; }
        public TimeSpan StartTime { get; set; }
        public int MaxParticipants { get; set; }
        public decimal Price { get; set; }

        // Navigation
        public Event? Event { get; set; }
        public Category? Category { get; set; }
    }
}
