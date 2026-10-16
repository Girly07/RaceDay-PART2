using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;
using RaceDay.API.Filters;
using RaceDay.API.Models;

namespace RaceDay.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResultsController : ControllerBase
    {
        private readonly RaceDayDbContext _context;

        public ResultsController(RaceDayDbContext context)
        {
            _context = context;
        }

        // POST: api/results (Organiser Only - Capture Results)
        [HttpPost]
        [AuthorizeRole("Organiser")]
        public async Task<IActionResult> CaptureResult(Result newResult)
        {
            // 1. Verify Enrolment exists
            var enrolment = await _context.Enrolments
                .Include(e => e.EventCategory)
                .FirstOrDefaultAsync(e => e.EnrolmentID == newResult.EnrolmentID);

            if (enrolment == null) return NotFound("Enrolment not found.");

            // 2. Verify the Organiser owns the event related to this enrolment
            var userId = HttpContext.Session.GetInt32("UserID");
            var ownsEvent = await _context.Events
                .AnyAsync(e => e.EventID == enrolment.EventCategory.EventID && e.OrganiserID == userId);

            if (!ownsEvent) return Forbid("You can only capture results for your own events.");

            // 3. Prevent duplicate results
            var existingResult = await _context.Results.AnyAsync(r => r.EnrolmentID == newResult.EnrolmentID);
            if (existingResult) return Conflict("A result already exists for this enrolment.");

            _context.Results.Add(newResult);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Result captured successfully", resultId = newResult.ResultID });
        }

        // GET: api/results/my-results (Participant Only)
        [HttpGet("my-results")]
        [AuthorizeRole("Participant")]
        public async Task<IActionResult> GetMyResults()
        {
            var userId = HttpContext.Session.GetInt32("UserID");

            var results = await _context.Results
                .Include(r => r.Enrolment)
                    .ThenInclude(e => e.EventCategory)
                        .ThenInclude(ec => ec.Event)
                .Where(r => r.Enrolment.ParticipantID == userId)
                .ToListAsync();

            return Ok(results);
        }

        // GET: api/results/event/5 (Organiser Only)
        [HttpGet("event/{eventId}")]
        [AuthorizeRole("Organiser")]
        public async Task<IActionResult> GetEventResults(int eventId)
        {
            var userId = HttpContext.Session.GetInt32("UserID");

            // Verify Organiser owns this event
            var ownsEvent = await _context.Events.AnyAsync(e => e.EventID == eventId && e.OrganiserID == userId);
            if (!ownsEvent) return Forbid("You do not own this event.");

            var results = await _context.Results
                .Include(r => r.Enrolment)
                    .ThenInclude(e => e.EventCategory)
                .Where(r => r.Enrolment.EventCategory.EventID == eventId)
                .ToListAsync();

            return Ok(results);
        }
    }
}