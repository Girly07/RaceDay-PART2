using System;
using RaceDay.API.Model;

namespace RaceDay.API.Models
{
    public class EventCategory
    {
        public int EventCategoryID { get; set; }
        public int EventID { get; set; }
        public int CategoryID { get; set; }
        public TimeSpan StartTime { get; set; }
        public int MaxParticipants { get; set; }
        public decimal Price { get; set; }

        // Navigation properties
        public Event? Event { get; set; }
        public Category? Category { get; set; }
    }
}