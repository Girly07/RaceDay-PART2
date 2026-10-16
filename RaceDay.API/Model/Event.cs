namespace RaceDay.API.Model
{
    public class Event
    {
        public int EventID { get; set; }
        public int OrganiserID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime EventDate { get; set; }
        public string Location { get; set; } = string.Empty;
        public string Status { get; set; } = "Open";

        // Navigation
        public User? Organiser { get; set; }
        public ICollection<EventCategory>? EventCategories { get; set; }
    }
    public class Event
    {
        public int EventID { get; set; }
        public int OrganiserID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime EventDate { get; set; }
        public string Location { get; set; } = string.Empty;
        public string Status { get; set; } = "Open";

        // Navigation
        public User? Organiser { get; set; }
        public ICollection<EventCategory>? EventCategories { get; set; }
    }
}
