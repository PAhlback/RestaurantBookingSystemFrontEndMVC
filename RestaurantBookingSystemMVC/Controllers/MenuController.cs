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
        private readonly ILogger _logger;

        private string _baseUri = "https://localhost:7168/api/";

        public MenuController(HttpClient client, IMemoryCache cache, ILogger<MenuController> logger)
        {
            _httpClient = client;
            _jsonOptions = JsonHelper.JsonOptionsHelper();
            _cache = cache;
            _logger = logger;
        }

        public async Task<IActionResult> Index(string searchQuery)
        {
            try
            {
                List<MenuItem> menuItemList;

                // Check if menuItemList is cached. If yes, and no search has been entered, we return.
                _cache.TryGetValue("MenuItemList", out menuItemList);
                if (menuItemList != null && string.IsNullOrEmpty(searchQuery))
                    return View(menuItemList);

                // If cache is empty, we get the menuItemList from the API and also cache it.
                if (menuItemList == null)
                {
                    var response = await _httpClient.GetAsync(_baseUri + "menuitems");
                    var json = await response.Content.ReadAsStringAsync();
                    menuItemList = JsonSerializer.Deserialize<List<MenuItem>>(json, _jsonOptions);

                    _cache.Set("MenuItemList", menuItemList);
                }

                // Filter based on searchQuery, regardless of if the menuItemList was populated from cache or API. 
                if (menuItemList != null && !string.IsNullOrEmpty(searchQuery))
                {
                    int numericSearch = 0;
                    int.TryParse(searchQuery, out numericSearch);

                    menuItemList = menuItemList.Where(mi => 
                        mi.Name.ToLower().Contains(searchQuery.ToLower()) 
                        || mi.Price <= numericSearch
                        || mi.Category.Name.ToLower().Contains(searchQuery.ToLower())).ToList();
                }

                if (menuItemList.Count() > 0)
                {
                    return View(menuItemList);
                }
                else
                {
                    return View();
                }
            }
            catch (Exception ex) 
            {
                _logger.LogInformation("ERROR: {ex}", ex.Message);
            }

            return View();
        }
    }
}
