using Microsoft.AspNetCore.Mvc;
using RaceDay.MVC.Models;
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
            if (HttpContext.Session.GetString("UserRole") != "Organiser") return RedirectToAction("Login", "Account");

            var events = await _apiService.GetAsync<List<dynamic>>("api/events");
            return View(events ?? new List<dynamic>());
        }

        [HttpGet]
        public IActionResult CreateEvent() => View();

        [HttpPost]
        public async Task<IActionResult> CreateEvent(CreateEventViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var response = await _apiService.PostAsync("api/events", model);

            if (response.IsSuccessStatusCode) return RedirectToAction("Index");

            ModelState.AddModelError("", "Failed to create event. Make sure API is running.");
            return View(model);
        }
    }
}