using Microsoft.AspNetCore.Mvc;
using RaceDay.MVC.Services;

namespace RaceDay.MVC.Controllers
{
    public class OrganiserController : Controller
    {
        private readonly ApiService _apiService;

        public OrganiserController(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            // Security check: Make sure user is logged in and is an Organiser
            if (HttpContext.Session.GetString("UserRole") != "Organiser")
            {
                return RedirectToAction("Login", "Account");
            }

            // Fetch events created by this organiser
            var events = await _apiService.GetAsync<List<dynamic>>("api/events");

            // (Note: In a full app, you'd filter events by the logged-in OrganiserID here)

            return View(events);
        }
    }
}