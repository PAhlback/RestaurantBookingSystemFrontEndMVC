using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using RestaurantBookingSystemMVC.Helpers;
using RestaurantBookingSystemMVC.Models.MenuItem;
using System.Text.Json;

namespace RestaurantBookingSystemMVC.Controllers
{
    public class MenuController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;
        private readonly IMemoryCache _cache;

        private string _baseUri = "https://localhost:7168/api/";

        public MenuController(HttpClient client, IMemoryCache cache)
        {
            _httpClient = client;
            _jsonOptions = JsonHelper.JsonOptionsHelper();
            _cache = cache;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                List<MenuItem> menuItemList;
                _cache.TryGetValue("MenuItemList", out menuItemList);
                if (menuItemList != null) return View(menuItemList);

                var response = await _httpClient.GetAsync(_baseUri + "menuitems");
                var json = await response.Content.ReadAsStringAsync();
                menuItemList = JsonSerializer.Deserialize<List<MenuItem>>(json, _jsonOptions);

                if (menuItemList == null) return View();
                
                _cache.Set("MenuItemList", menuItemList);

                return View(menuItemList);
            }
            catch (Exception ex) { }

            return View();
        }
    }
}
