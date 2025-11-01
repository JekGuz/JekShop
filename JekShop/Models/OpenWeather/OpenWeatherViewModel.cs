namespace JekShop.Models.OpenWeather
{
    public class OpenWeatherViewModel
    {
        public string? City { get; set; }
        public string? Country { get; set; }
        public string? Units { get; set; }

        public double? Temp { get; set; }
        public double? FeelsLike { get; set; }
        public int? Humidity { get; set; }
        public double? WindSpeed { get; set; }
        public string? Description { get; set; }
        public string? IconUrl { get; set; }

        // добавим список на 1 или 3 дня
        public List<DailyForecastVm> Days { get; set; } = new();
    }

    public class DailyForecastVm
    {
        public DateOnly Date { get; set; }
        public double? Min { get; set; }
        public double? Max { get; set; }
        public string? Description { get; set; }
        public string? IconUrl { get; set; }
    }
}