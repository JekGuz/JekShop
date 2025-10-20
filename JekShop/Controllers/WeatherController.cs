using JekShop.Core.Dto;
using JekShop.Core.ServiceInterface;
using JekShop.Models.Weather;
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
        public async Task<IActionResult> SearchCity(AccuWeatherSearchModel model)
        {
            if(ModelState.IsValid)
            {
                return RedirectToAction("city", "weather", new { city = model.CityName });
            }
            return View(model);

            //var result = await _WeatherForecastServices.AccuWeatherResult(dto);
            //return View("Index", result);
        }
        [HttpGet]
        public IActionResult City(string city)
        {
            AccuLocationWeatherResultDto dto = new();
            dto.CityName = city;


            _WeatherForecastServices.AccuWeatherResult(dto);

            AccuWeatherViewModel vm = new();

            vm.TemoMectricValueUnit = dto.TemoMectricValueUnit;
            vm.Text = dto.Text;
            vm.EndDate = dto.EndDate;


            return View(vm);
        }

    }
}
