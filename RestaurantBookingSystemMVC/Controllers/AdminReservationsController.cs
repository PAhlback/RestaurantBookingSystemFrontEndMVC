using Microsoft.AspNetCore.Mvc;
using RestaurantBookingSystemMVC.Helpers;
using RestaurantBookingSystemMVC.Models.Reservation;
using System.Net.Http;
using System.Resources;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RestaurantBookingSystemMVC.Controllers
{
    public class AdminReservationsController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<HomeController> _logger;
        private readonly JsonSerializerOptions _serializerOptions;

        private string _baseUri = "https://localhost:7168/api/";

        public AdminReservationsController(HttpClient client, ILogger<HomeController> logger)
        {
            _httpClient = client;
            _logger = logger;
            _serializerOptions = JsonHelper.JsonOptionsHelper();
        }

        public async Task<IActionResult> Index()
        {
            Console.Out.WriteLine("Reservations index");
            var response = await _httpClient.GetAsync(_baseUri + "reservations");

            var json = await response.Content.ReadAsStringAsync();

            var reservations = JsonSerializer.Deserialize<IEnumerable<Reservation>>(json, _serializerOptions);

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

        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpClient.GetAsync(_baseUri + $"reservations/{id}");

            var json = await response.Content.ReadAsStringAsync();
            
            var reservation = JsonSerializer.Deserialize<Reservation>(json, _serializerOptions);

            return View(reservation);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Reservation reservation)
        {
            var updateReservation = new UpdateReservationDTO
            {
                NumberOfGuests = reservation.NumberOfGuests,
                CustomerEmail = reservation.Customer.Email,
                CustomerName = reservation.Customer.Name,
                CustomerPhone = reservation.Customer.Phone,
                DateAndTime = reservation.DateAndTime,
                TableId = reservation.Table.Id,
                TableNumber = reservation.Table.TableNumber,
            };

            var json = JsonSerializer.Serialize(updateReservation);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            await _httpClient.PutAsync(_baseUri + $"reservations/{reservation.Id}", content);

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
