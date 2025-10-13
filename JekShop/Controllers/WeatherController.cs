using JekShop.Core.Dto;
using JekShop.Core.ServiceInterface;
using Microsoft.AspNetCore.Mvc;

namespace JekShop.Controllers
{
    public class WeatherController : Controller
    {
        private readonly IWeatherForecastServices _WeatherForecastServices;
        public WeatherController
        (
            IWeatherForecastServices weatherForecastServices
            )
        {
            _WeatherForecastServices = weatherForecastServices;
        }
        public IActionResult Index()
        {
            return View();
        }

        //teha action SearchCity
        [HttpPost]
        public async Task<IActionResult> SearchCity(AccuLocationWeatherResultDto dto)
        {
            var result = await _WeatherForecastServices.AccuWeatherResult(dto);
            return View("Index", result);
        }
    }
}
