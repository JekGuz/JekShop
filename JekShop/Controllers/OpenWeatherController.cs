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

        public OpenWeatherController(IOpenWeatherService svc) => _svc = svc;

        [HttpGet]
        public IActionResult Index() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SearchCity(string city, string units = "metric")
        {
            city = city?.Trim();
            if (string.IsNullOrWhiteSpace(city))
            {
                TempData["ow_error"] = "Введите название города.";
                return RedirectToAction(nameof(Index));
            }

            units = (units == "imperial") ? "imperial" : "metric";
            return RedirectToAction(nameof(City), new { city, units });
        }

        [HttpGet]
        public async Task<IActionResult> City(string city, string units = "metric")
        {
            var dto = await _svc.GetCurrentByCityAsync(city, units);
            if (dto == null)
            {
                TempData["ow_error"] = "City not found or API error.";
                return RedirectToAction(nameof(Index));
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SearchCity(string city, string units = "metric", int days = 1)
        {
            city = city?.Trim();
            if (string.IsNullOrWhiteSpace(city))
            {
                TempData["ow_error"] = "Введите название города.";
                return RedirectToAction(nameof(Index));
            }
            units = (units == "imperial") ? "imperial" : "metric";
            days = (days == 3) ? 3 : 1;
            return RedirectToAction(nameof(City), new { city, units, days });
        }

        [HttpGet]
        public async Task<IActionResult> City(string city, string units = "metric", int days = 1)
        {
            days = (days == 3) ? 3 : 1;

            // current
            var cur = await _svc.GetCurrentByCityAsync(city, units);
            if (cur == null)
            {
                TempData["ow_error"] = "City not found or API error.";
                return RedirectToAction(nameof(Index));
            }

            var vm = new OpenWeatherViewModel
            {
                City = cur.name ?? city,
                Country = cur.sys?.country ?? "",
                Temp = (double?)cur.main?.temp,
                FeelsLike = (double?)cur.main?.feels_like,
                Humidity = cur.main?.humidity,
                WindSpeed = cur.wind?.speed,
                Description = cur.weather?.FirstOrDefault()?.description ?? "",
                IconUrl = string.IsNullOrWhiteSpace(cur.weather?.FirstOrDefault()?.icon)
                            ? ""
                            : $"https://openweathermap.org/img/wn/{cur.weather.First().icon}@2x.png",
                Units = units
            };

            // forecast
            var fc = await _svc.GetForecastByCityAsync(city, units);
            if (fc?.list != null && fc.list.Count > 0)
            {
                // сгруппировать по датам
                var groups = fc.list
                    .Where(x => x.dt.HasValue)
                    .GroupBy(x => DateOnly.FromDateTime(DateTimeOffset.FromUnixTimeSeconds(x.dt!.Value).DateTime.Date))
                    .OrderBy(g => g.Key)
                    .Take(days); // 1 или 3

                foreach (var g in groups)
                {
                    var min = g.Min(x => x.main?.temp_min);
                    var max = g.Max(x => x.main?.temp_max);
                    var w = g
                        .Select(x => x.weather?.FirstOrDefault())
                        .FirstOrDefault(x => x != null);

                    vm.Days.Add(new DailyForecastVm
                    {
                        Date = g.Key,
                        Min = min,
                        Max = max,
                        Description = w?.description ?? w?.main,
                        IconUrl = string.IsNullOrWhiteSpace(w?.icon) ? "" : $"https://openweathermap.org/img/wn/{w!.icon}.png"
                    });
                }
            }

            return View(vm);
        }

    }
}
