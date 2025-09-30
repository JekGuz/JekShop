using Microsoft.AspNetCore.Mvc;

namespace JekShop.Controllers
{
    public class RealEstateController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
