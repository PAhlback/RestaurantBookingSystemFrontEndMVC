using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantBookingSystemMVC.Helpers;
using RestaurantBookingSystemMVC.Models;
using RestaurantBookingSystemMVC.Models.MenuItem;
using RestaurantBookingSystemMVC.Models.Reservation;
using System.Security.Claims;
using System.Text.Json;

namespace RestaurantBookingSystemMVC.Controllers
{
    public class AdminController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;
        private readonly ILogger<HomeController> _logger;

        public AdminController(HttpClient client, ILogger<HomeController> logger)
        {
            _httpClient = client;
            _httpClient.BaseAddress = new Uri("https://localhost:7168/api/");
            _logger = logger;
            _jsonOptions = JsonHelper.JsonOptionsHelper();
        }

        [Authorize]
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View();
            }
            else
            {
                // User is not authenticated
                return RedirectToAction("Login", "Admin");
            }
        }

        public async Task<IActionResult> Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            var response = await _httpClient.PostAsJsonAsync("users/login", loginDTO);

            if (!response.IsSuccessStatusCode)
                return View(loginDTO);

            var json = await response.Content.ReadAsStringAsync();
            AdminUser? user = JsonSerializer.Deserialize<AdminUser>(json, _jsonOptions);
            if (user == null)
                return BadRequest("User does not exist.");

            var cookies = response.Headers.GetValues("Set-Cookie");
            foreach (var cookie in cookies)
            {
                Response.Headers.Append("Set-Cookie", cookie);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}")
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

            return RedirectToAction("Index", "Admin");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            var response = await _httpClient.PostAsync("users/logout", null);

            if (response.IsSuccessStatusCode)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return RedirectToAction("Login", "Admin");
            }

            return BadRequest("Logout failed");
        }
    }
}
