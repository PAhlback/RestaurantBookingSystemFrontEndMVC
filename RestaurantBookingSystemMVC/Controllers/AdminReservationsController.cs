﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantBookingSystemMVC.Helpers;
using RestaurantBookingSystemMVC.Models.Reservation;
using System.Net;
using System.Net.Http;
using System.Resources;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RestaurantBookingSystemMVC.Controllers
{
    [Authorize]
    public class AdminReservationsController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<HomeController> _logger;
        private readonly JsonSerializerOptions _serializerOptions;

        private string _baseUri = "https://localhost:7168/api/";

        public AdminReservationsController(HttpClient httpClient, ILogger<HomeController> logger)
        {
            _logger = logger;
            _serializerOptions = JsonHelper.JsonOptionsHelper();
            _httpClient = httpClient;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                _logger.LogInformation("---- INFO AdminReservationsController: Getting reservations");

                var response = await _httpClient.GetAsync(_baseUri + "reservations");

                //if (!response.IsSuccessStatusCode)
                //    return RedirectToAction("Login", "Admin");

                var json = await response.Content.ReadAsStringAsync();

                var reservations = JsonSerializer.Deserialize<List<Reservation>>(json, _serializerOptions);

                _logger.LogInformation("---- INFO AdminReservationsController: Returning reservations {Description}", json);

                return View(reservations);
            }
            catch (Exception ex) { }

            return View();
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
