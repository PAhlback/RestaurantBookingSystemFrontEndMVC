using Microsoft.AspNetCore.Mvc;

namespace RestaurantBookingSystemMVC.Controllers
{
    public class MenuController : Controller
    {
        public IActionResult Index()
        {


            return View();
        }
    }
}
