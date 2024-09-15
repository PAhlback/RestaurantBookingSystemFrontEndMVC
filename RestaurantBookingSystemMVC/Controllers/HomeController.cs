using Microsoft.AspNetCore.Mvc;
using RestaurantBookingSystemMVC.Models;
using RestaurantBookingSystemMVC.Models.MenuItem;
using System.Diagnostics;
using System.Text.Json;

namespace RestaurantBookingSystemMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<HomeController> _logger;

        private string _baseUri = "https://localhost:7168/api/";

        public HomeController(HttpClient client, ILogger<HomeController> logger)
        {
            _httpClient = client;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var response = await _httpClient.GetAsync(_baseUri + "menuitems/popular");

                var json = await response.Content.ReadAsStringAsync();

                // The deserializer was not mapping correctly due to case sensitivity.
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                
                var popularItemsList = JsonSerializer.Deserialize<List<MenuItem>>(json, options);

                if (popularItemsList == null)
                {
                    return View();
                }

                return View(popularItemsList);
            }
            catch (Exception ex) { }

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
