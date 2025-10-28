using JekShop.Core.ServiceInterface;
using Microsoft.AspNetCore.Mvc;

public class OpenWeatherController : Controller
{
    private readonly IOpenWeatherService _svc;
    public OpenWeatherController(IOpenWeatherService svc) => _svc = svc;

    [HttpGet]
    public async Task<IActionResult> Index(string city = "Tallinn", string units = "metric")
    {
        var dto = await _svc.GetCurrentByCityAsync(city, units);
        return View(dto);
    }
}