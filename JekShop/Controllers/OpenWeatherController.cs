using JekShop.Core.ServiceInterface;
using JekShop.Models.OpenWeather;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace JekShop.Controllers
{
    public class OpenWeatherController : Controller
    {
        private readonly IOpenWeatherService _svc;

        public OpenWeatherController(IOpenWeatherService svc)
        {
            _svc = svc;
        }

        // Стартовая страница с формой
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        // Пост с формы -> редирект с параметрами
        [HttpPost]
        public IActionResult SearchCity(string city, string units = "metric")
        {
            if (string.IsNullOrWhiteSpace(city))
            {
                ModelState.AddModelError(string.Empty, "Введите название города");
                return View("Index");
            }

            return RedirectToAction(nameof(City), new { city, units });
        }

        // Страница результата
        [HttpGet]
        public async Task<IActionResult> City(string city, string units = "metric")
        {
            var dto = await _svc.GetCurrentByCityAsync(city, units);
            if (dto == null)
            {
                return View("Index");
            }

            var w = dto.weather?.FirstOrDefault();

            var vm = new OpenWeatherViewModel
            {
                City = dto.name ?? city,
                Country = dto.sys?.country ?? "",
                Temp = (double?)dto.main?.temp,
                FeelsLike = (double?)dto.main?.feels_like,
                Humidity = dto.main?.humidity,
                WindSpeed = dto.wind?.speed,
                Description = w?.description ?? w?.main ?? "",
                IconUrl = string.IsNullOrWhiteSpace(w?.icon) ? "" : $"https://openweathermap.org/img/wn/{w!.icon}@2x.png",
                Units = units
            };

            return View(vm);
        }
    }
}
