using System;
using RaceDay.API.Model;

namespace RaceDay.API.Models
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

        
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public User? Organiser { get; set; }
    }
}