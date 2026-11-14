using Microsoft.AspNetCore.Mvc;
using RaceDay.MVC.Services;

namespace RaceDay.MVC.Controllers
{
    public class ParticipantController : Controller
    {
        private readonly ApiService _apiService;

        public ParticipantController(ApiService apiService)
        {
            _apiService = apiService;
        }

        // View My Enrolments
        public async Task<IActionResult> MyEnrolments()
        {
            if (HttpContext.Session.GetInt32("UserID") == null) return RedirectToAction("Login", "Account");

            var enrolments = await _apiService.GetAsync<List<dynamic>>("api/enrolments/my-enrolments");
            return View(enrolments ?? new List<dynamic>());
        }

        // View My Results
        public async Task<IActionResult> MyResults()
        {
            if (HttpContext.Session.GetInt32("UserID") == null) return RedirectToAction("Login", "Account");

            var results = await _apiService.GetAsync<List<dynamic>>("api/results/my-results");
            return View(results ?? new List<dynamic>());
        }

        // Enrol in an event category (POST)
        [HttpPost]
        public async Task<IActionResult> Enrol(int eventCategoryId)
        {
            if (HttpContext.Session.GetInt32("UserID") == null) return RedirectToAction("Login", "Account");

            // The API expects an object with EventCategoryID
            var data = new { EventCategoryID = eventCategoryId };
            var response = await _apiService.PostAsync("api/enrolments", data);

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Successfully enrolled!";
                return RedirectToAction("MyEnrolments");
            }

            TempData["Error"] = "Failed to enrol. You may already be enrolled.";
            return RedirectToAction("Index", "Home");
        }
    }
}