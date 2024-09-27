using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantBookingSystemMVC.Helpers;
using RestaurantBookingSystemMVC.Models.MenuItem;
using RestaurantBookingSystemMVC.Models.MenuItem.DTOs;
using RestaurantBookingSystemMVC.Models.Reservation;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace RestaurantBookingSystemMVC.Controllers
{
    [Authorize]
    public class AdminMenuController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUri = "https://localhost:7168/api/";
        private readonly JsonSerializerOptions _serializerOptions;

        public AdminMenuController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _serializerOptions = JsonHelper.JsonOptionsHelper();
        }

        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync(_baseUri + "menuitems");

            var json = await response.Content.ReadAsStringAsync();
            var jsonOptions = JsonHelper.JsonOptionsHelper();

            var menuItems = JsonSerializer.Deserialize<List<MenuItem>>(json, jsonOptions);

            return View(menuItems);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(MenuItemDTO newMenuItem)
        {
            if (!ModelState.IsValid) { return View(newMenuItem); }

            var json = JsonSerializer.Serialize(newMenuItem);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(_baseUri + "menuitems", content);

            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpClient.GetAsync(_baseUri + $"menuitems/{id}");

            var json = await response.Content.ReadAsStringAsync();

            var menuItem = JsonSerializer.Deserialize<MenuItem>(json, _serializerOptions);

            return View(menuItem);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(MenuItem menuItem)
        {
            var menuItemDTO = new MenuItemDTO
            {
                Name = menuItem.Name,
                Description = menuItem.Description,
                Price = menuItem.Price,
                IsAvailable = menuItem.IsAvailable,
                CategoryFK = menuItem.Category?.Id != null ? menuItem.Category.Id : 0
            };

            var json = JsonSerializer.Serialize(menuItemDTO);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            await _httpClient.PutAsync(_baseUri + $"menuitems/{menuItem.Id}", content);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync(_baseUri + $"menuitems/{id}");

            return RedirectToAction("Index");
        }
    }
}
