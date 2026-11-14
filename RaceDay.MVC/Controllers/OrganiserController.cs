using Microsoft.AspNetCore.Mvc;
using RaceDay.MVC.Models;
using RaceDay.MVC.Services;
using System.Text.Json;

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
            ModelState.AddModelError("", "Failed to create event.");
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EventEnrolments(int id)
        {
            if (HttpContext.Session.GetString("UserRole") != "Organiser") return RedirectToAction("Login", "Account");
            var enrolments = await _apiService.GetAsync<List<dynamic>>($"api/enrolments/event/{id}");
            ViewBag.EventId = id;
            return View(enrolments ?? new List<dynamic>());
        }

        // NEW: Capture Result Form
        [HttpGet]
        public IActionResult CaptureResult(int enrolmentId)
        {
            ViewBag.EnrolmentId = enrolmentId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CaptureResult(int enrolmentId, string finishTime, int position)
        {
            var data = new
            {
                EnrolmentID = enrolmentId,
                FinishTime = finishTime,
                Position = position,
                Status = "Finished"
            };

            var response = await _apiService.PostAsync("api/results", data);

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Result captured!";
                return RedirectToAction("Index"); // Go back to dashboard
            }

            ModelState.AddModelError("", "Failed to capture result. It might already exist.");
            return View();
        }
    }
}