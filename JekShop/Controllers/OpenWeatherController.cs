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

        // Стартовая страница
        [HttpGet]
        public IActionResult Index() => View();

        // POST из формы: город + единицы + дни (1/3)
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

        // Страница результата
        [HttpGet]
        public async Task<IActionResult> City(string city, string units = "metric", int days = 1)
        {
            days = (days == 3) ? 3 : 1;

            // Текущая погода
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
                                : $"https://openweathermap.org/img/wn/{cur.weather!.First()!.icon}@2x.png",
                Units = units
            };

            // Прогноз на 1/3 дня
            var fc = await _svc.GetForecastByCityAsync(city, units);
            if (fc?.list != null && fc.list.Count > 0)
            {
                var groups = fc.list
                    .Where(x => x.dt.HasValue)
                    .GroupBy(x => DateOnly.FromDateTime(DateTimeOffset.FromUnixTimeSeconds(x.dt!.Value).DateTime.Date))
                    .OrderBy(g => g.Key)
                    .Take(days);

                foreach (var g in groups)
                {
                    var min = g.Min(x => x.main?.temp_min);
                    var max = g.Max(x => x.main?.temp_max);
                    var w = g.Select(x => x.weather?.FirstOrDefault())
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
