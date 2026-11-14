using Microsoft.AspNetCore.Mvc;
using RaceDay.MVC.Services;

namespace RaceDay.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApiService _apiService;

        public HomeController(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            var events = await _apiService.GetAsync<List<dynamic>>("api/events");
            ViewBag.IsApiOnline = events != null;
            return View(events ?? new List<dynamic>());
        }

        
        public async Task<IActionResult> EventDetails(int id)
        {
            var ev = await _apiService.GetAsync<dynamic>($"api/events/{id}");
            var categories = await _apiService.GetAsync<List<dynamic>>($"api/categories/event/{id}");

            if (ev == null) return NotFound();

            ViewBag.Categories = categories;
            return View(ev);
        }
    }
}