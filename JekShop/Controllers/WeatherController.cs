using Microsoft.AspNetCore.Mvc;

namespace JekShop.Controllers
{
    public class WeatherController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
