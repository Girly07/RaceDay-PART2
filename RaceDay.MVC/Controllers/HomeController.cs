using Microsoft.AspNetCore.Mvc;
using RaceDay.MVC.Services;
using System.Diagnostics;
using RaceDay.MVC.Models;

namespace RaceDay.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApiService _apiService;

        // Inject the ApiService here
        public HomeController(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            // Call the API
            var events = await _apiService.GetAsync<List<dynamic>>("api/events");

            // Set a message so we know if it worked
            ViewBag.ApiStatus = events != null ? "Successfully connected to API!" : "Failed to connect to API.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}