namespace JekShop.Models.OpenWeather
{
    public class OpenWeatherViewModel
    {
        public string? City { get; set; }
        public string? Country { get; set; }

        public double? Temp { get; set; }
        public double? FeelsLike { get; set; }
        public int? Humidity { get; set; }
        public double? WindSpeed { get; set; }

        public string? Description { get; set; }
        public string? IconUrl { get; set; } // https://openweathermap.org/img/wn/{icon}@2x.png  очень мило будет :)
        public string? Units { get; set; } // для подписи °C / °F
    }
}
