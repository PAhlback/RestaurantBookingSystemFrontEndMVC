﻿using Microsoft.AspNetCore.Mvc;
using RestaurantBookingSystemMVC.Helpers;
using RestaurantBookingSystemMVC.Models.MenuItem;
using RestaurantBookingSystemMVC.Models.Reservation;
using System.Text.Json;

namespace RestaurantBookingSystemMVC.Controllers
{
    public class AdminController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<HomeController> _logger;

        private string _baseUri = "https://localhost:7168/api/";

        public AdminController(HttpClient client, ILogger<HomeController> logger)
        {
            _httpClient = client;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Menu()
        {
            var response = await _httpClient.GetAsync(_baseUri + "menuitems");

            var json = await response.Content.ReadAsStringAsync();
            var jsonOptions = JsonHelper.JsonOptionsHelper();

            var menuItems = JsonSerializer.Deserialize<List<MenuItem>>(json, jsonOptions);

            return View(menuItems);
        }
    }
}
