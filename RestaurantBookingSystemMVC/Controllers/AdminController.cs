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
        private readonly ILogger<HomeController> _logger;

        public AdminController(HttpClient client, ILogger<HomeController> logger)
        {
            _httpClient = client;
            _httpClient.BaseAddress = new Uri("https://localhost:7168/api/");
            _logger = logger;
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

            var cookies = response.Headers.GetValues("Set-Cookie");

            foreach (var cookie in cookies)
            {
                Response.Headers.Append("Set-Cookie", cookie);
            }

            // This part is important
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, loginDTO.Email) // Assuming the DTO has a Username property
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

            return RedirectToAction("Index", "Admin");
        }
    }
}
