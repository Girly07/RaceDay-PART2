using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;
using RaceDay.API.Filters;
using RaceDay.API.Models;

namespace RaceDay.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly RaceDayDbContext _context;

        public EventsController(RaceDayDbContext context)
        {
            _context = context;
        }

        // GET: api/events (Public - anyone can view events)
        [HttpGet]
        public async Task<IActionResult> GetEvents()
        {
            var events = await _context.Events
                .Include(e => e.Organiser)
                .ToListAsync();
            return Ok(events);
        }

        // GET: api/events/5 (Public)
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEvent(int id)
        {
            var raceEvent = await _context.Events
                .Include(e => e.Organiser)
                .FirstOrDefaultAsync(e => e.EventID == id);

            if (raceEvent == null)
                return NotFound("Event not found.");

            return Ok(raceEvent);
        }

        // POST: api/events (Organiser Only)
        [HttpPost]
        [AuthorizeRole("Organiser")]
        public async Task<IActionResult> CreateEvent(Event newEvent)
        {
            // Get the logged-in Organiser's ID from the session
            var userId = HttpContext.Session.GetInt32("UserID");
            if (userId == null) return Unauthorized();

            newEvent.OrganiserID = userId.Value;
            newEvent.CreatedAt = DateTime.Now;
            newEvent.Status = "Open";

            _context.Events.Add(newEvent);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEvent), new { id = newEvent.EventID }, newEvent);
        }

        // PUT: api/events/5 (Organiser Only)
        [HttpPut("{id}")]
        [AuthorizeRole("Organiser")]
        public async Task<IActionResult> UpdateEvent(int id, Event updatedEvent)
        {
            var userId = HttpContext.Session.GetInt32("UserID");
            var existingEvent = await _context.Events.FindAsync(id);

            if (existingEvent == null) return NotFound("Event not found.");

            // Security: Ensure the Organiser owns this event
            if (existingEvent.OrganiserID != userId)
                return Forbid("You can only edit your own events.");

            existingEvent.Name = updatedEvent.Name;
            existingEvent.Description = updatedEvent.Description;
            existingEvent.EventDate = updatedEvent.EventDate;
            existingEvent.Location = updatedEvent.Location;
            existingEvent.Status = updatedEvent.Status;

            await _context.SaveChangesAsync();
            return Ok(existingEvent);
        }

        // DELETE: api/events/5 (Organiser Only)
        [HttpDelete("{id}")]
        [AuthorizeRole("Organiser")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserID");
            var existingEvent = await _context.Events.FindAsync(id);

            if (existingEvent == null) return NotFound("Event not found.");
            if (existingEvent.OrganiserID != userId) return Forbid("You can only delete your own events.");

            _context.Events.Remove(existingEvent);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}