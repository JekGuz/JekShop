using System.Threading.Tasks;
using JekShop.Core.Dto;
using JekShop.Core.ServiceInterface;
using JekShop.Models.Weather;
using Microsoft.AspNetCore.Mvc;

namespace JekShop.Controllers
{
    public class WeatherController : Controller
    {
        private readonly IWeatherForecastServices _weatherForecastServices;

        public WeatherController(IWeatherForecastServices weatherForecastServices)
        {
            _weatherForecastServices = weatherForecastServices;
        }

        [HttpGet]
        public IActionResult Index() => View();

        // teha action SearchCity
        [HttpPost]
        public IActionResult SearchCity(AccuWeatherSearchModel model)
        {
            if (!ModelState.IsValid)
                return View("Index", model);

            return RedirectToAction(nameof(City), new { city = model.CityName });
        }

        [HttpGet]
        public async Task<IActionResult> City(string city)
        {
            var dto = new AccuLocationWeatherResultDto { CityName = city };

            // ВАЖНО: ждём результат!
            var result = await _weatherForecastServices.AccuWeatherResultWebClient(dto);

            var vm = new AccuWeatherViewModel
            {
                CityName = result.CityName,
                EffectiveDate = result.EffectiveDate,
                EffectiveEpochDate = result.EffectiveEpochDate,
                Severity = result.Severity,
                Text = result.Text,
                Category = result.Category,
                EndDate = result.EndDate,
                EndEpochDate = result.EndEpochDate,
                DailyForecastsDate = result.DailyForecastsDate,
                DailyForecastsEpochDate = result.DailyForecastsEpochDate,

                TempMinValue = result.TempMinValue,
                TempMinUnit = result.TempMinUnit,
                TempMinUnitType = result.TempMinUnitType,

                TempMaxValue = result.TempMaxValue,
                TempMaxUnit = result.TempMaxUnit,
                TempMaxUnitType = result.TempMaxUnitType,

                DayIcon = result.DayIcon,
                DayIconPhrase = result.DayIconPhrase,
                DayHasPrecipitation = result.DayHasPrecipitation,
                DayPrecipitationType = result.DayPrecipitationType,
                DayPrecipitationIntensity = result.DayPrecipitationIntensity,

                NightIcon = result.NightIcon,
                NightIconPhrase = result.NightIconPhrase,
                NightHasPrecipitation = result.NightHasPrecipitation,
                NightPrecipitationType = result.NightPrecipitationType,
                NightPrecipitationIntensity = result.NightPrecipitationIntensity,

                MobileLink = result.MobileLink,
                Link = result.Link
            };

            return View(vm);
        }
    }
}
