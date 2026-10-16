using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;
using RaceDay.API.Filters;
using RaceDay.API.Models;

namespace RaceDay.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly RaceDayDbContext _context;

        public CategoriesController(RaceDayDbContext context)
        {
            _context = context;
        }

        // GET: api/categories (Public)
        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            return Ok(await _context.Categories.ToListAsync());
        }

        // POST: api/categories (Organiser Only)
        [HttpPost]
        [AuthorizeRole("Organiser")]
        public async Task<IActionResult> CreateCategory(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return Ok(category);
        }

        // POST: api/categories/link (Organiser Only) - Links a Category to an Event
        [HttpPost("link")]
        [AuthorizeRole("Organiser")]
        public async Task<IActionResult> LinkCategoryToEvent(EventCategory link)
        {
            var userId = HttpContext.Session.GetInt32("UserID");

            // Ensure the event belongs to this organiser
            var eventExists = await _context.Events.AnyAsync(e => e.EventID == link.EventID && e.OrganiserID == userId);
            if (!eventExists) return Forbid("You do not own this event.");

            // Ensure category exists
            var categoryExists = await _context.Categories.AnyAsync(c => c.CategoryID == link.CategoryID);
            if (!categoryExists) return NotFound("Category not found.");

            _context.EventCategories.Add(link);
            await _context.SaveChangesAsync();
            return Ok(link);
        }

        // GET: api/categories/event/5 (Public) - Get all categories linked to a specific event
        [HttpGet("event/{eventId}")]
        public async Task<IActionResult> GetEventCategories(int eventId)
        {
            var categories = await _context.EventCategories
                .Include(ec => ec.Category)
                .Where(ec => ec.EventID == eventId)
                .ToListAsync();
            return Ok(categories);
        }
    }
}