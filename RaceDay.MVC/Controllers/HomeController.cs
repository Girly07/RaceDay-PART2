using Microsoft.AspNetCore.Mvc;
using RaceDay.MVC.Services;
using System.Diagnostics;
using RaceDay.MVC.Models;

namespace RaceDay.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApiService _apiService;
        private readonly ILogger<HomeController> _logger;

        // Inject your ApiService here
        public HomeController(ApiService apiService, ILogger<HomeController> logger)
        {
            _apiService = apiService;
            _logger = logger;
        }

        // This is your landing page
        public async Task<IActionResult> Index()
        {
            // Call the API's GET /api/events endpoint
            // We use 'dynamic' here temporarily just to test the connection
            var events = await _apiService.GetAsync<List<dynamic>>("api/events");

            // If the API is running, this will have data.
            // If the API is off, this will be null.
            return View(events);
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