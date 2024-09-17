using Microsoft.AspNetCore.Mvc;
using RestaurantBookingSystemMVC.Models.MenuItem;
using System.Text.Json;

namespace RestaurantBookingSystemMVC.Controllers
{
    public class MenuController : Controller
    {
        private readonly HttpClient _httpClient;

        private string _baseUri = "https://localhost:7168/api/";

        public MenuController(HttpClient client)
        {
            _httpClient = client;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var response = await _httpClient.GetAsync(_baseUri + "menuitems");

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var json = await response.Content.ReadAsStringAsync();

                var menuItemList = JsonSerializer.Deserialize<List<MenuItem>>(json, options);

                if (menuItemList == null)
                {
                    return View();
                }

                return View(menuItemList);

            }
            catch (Exception ex) { }

            return View();
        }
    }
}
