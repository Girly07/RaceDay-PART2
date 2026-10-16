using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;
using RaceDay.API.Filters;
using RaceDay.API.Models;

namespace RaceDay.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrolmentsController : ControllerBase
    {
        private readonly RaceDayDbContext _context;

        public EnrolmentsController(RaceDayDbContext context)
        {
            _context = context;
        }

        // POST: api/enrolments (Participant Only)
        [HttpPost]
        [AuthorizeRole("Participant")]
        public async Task<IActionResult> Enrol(Enrolment newEnrolment)
        {
            var userId = HttpContext.Session.GetInt32("UserID");
            if (userId == null) return Unauthorized();

            // 1. Ensure the EventCategory exists
            var eventCategory = await _context.EventCategories
                .Include(ec => ec.Event)
                .FirstOrDefaultAsync(ec => ec.EventCategoryID == newEnrolment.EventCategoryID);

            if (eventCategory == null) return NotFound("Event Category not found.");

            // 2. Check if the event is still open
            if (eventCategory.Event?.Status == "Closed" || eventCategory.Event?.Status == "Cancelled")
                return BadRequest("This event is no longer accepting enrolments.");

            // 3. Check if Participant is already enrolled in this specific EventCategory
            var alreadyEnrolled = await _context.Enrolments
                .AnyAsync(e => e.ParticipantID == userId && e.EventCategoryID == newEnrolment.EventCategoryID);

            if (alreadyEnrolled) return Conflict("You are already enrolled in this category.");

            // 4. Create the Enrolment
            newEnrolment.ParticipantID = userId.Value;
            newEnrolment.EnrolmentDate = DateTime.Now;
            newEnrolment.Status = "Confirmed";
            newEnrolment.PaymentStatus = "Unpaid";

            _context.Enrolments.Add(newEnrolment);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Enrolment successful", enrolmentId = newEnrolment.EnrolmentID });
        }

        // GET: api/enrolments/my-enrolments (Participant Only)
        [HttpGet("my-enrolments")]
        [AuthorizeRole("Participant")]
        public async Task<IActionResult> GetMyEnrolments()
        {
            var userId = HttpContext.Session.GetInt32("UserID");

            var enrolments = await _context.Enrolments
                .Include(e => e.EventCategory)
                    .ThenInclude(ec => ec.Event)
                .Include(e => e.EventCategory)
                    .ThenInclude(ec => ec.Category)
                .Where(e => e.ParticipantID == userId)
                .ToListAsync();

            return Ok(enrolments);
        }

        // GET: api/enrolments/event/5 (Organiser Only)
        [HttpGet("event/{eventId}")]
        [AuthorizeRole("Organiser")]
        public async Task<IActionResult> GetEventEnrolments(int eventId)
        {
            var userId = HttpContext.Session.GetInt32("UserID");

            // Verify the organiser owns this event
            var ownsEvent = await _context.Events.AnyAsync(e => e.EventID == eventId && e.OrganiserID == userId);
            if (!ownsEvent) return Forbid("You do not own this event.");

            var enrolments = await _context.Enrolments
                .Include(e => e.EventCategory)
                .Where(e => e.EventCategory.EventID == eventId)
                .ToListAsync();

            return Ok(enrolments);
        }
    }
}