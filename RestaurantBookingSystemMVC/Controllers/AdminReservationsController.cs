using Microsoft.AspNetCore.Mvc;
using RestaurantBookingSystemMVC.Helpers;
using RestaurantBookingSystemMVC.Models.Reservation;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RestaurantBookingSystemMVC.Controllers
{
    public class AdminReservationsController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<HomeController> _logger;

        private string _baseUri = "https://localhost:7168/api/";

        public AdminReservationsController(HttpClient client, ILogger<HomeController> logger)
        {
            _httpClient = client;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            Console.Out.WriteLine("Reservations index");
            var response = await _httpClient.GetAsync(_baseUri + "reservations");

            var json = await response.Content.ReadAsStringAsync();
            var jsonOptions = JsonHelper.JsonOptionsHelper();

            var reservations = JsonSerializer.Deserialize<IEnumerable<Reservation>>(json, jsonOptions);

            return View(reservations);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(NewReservationDTO newReservation)
        {
            if (!ModelState.IsValid) { return View(newReservation); }

            var json = JsonSerializer.Serialize(newReservation);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(_baseUri + "reservations", content);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync(_baseUri + $"reservations/{id}");

            return RedirectToAction("Index");
        }
    }
}
