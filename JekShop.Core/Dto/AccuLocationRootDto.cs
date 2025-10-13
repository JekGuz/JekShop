using System.Data;

namespace JekShop.Core.Dto
{
    public class AccuLocationRootDto
    {
        public string? LocalObservatioDateTime { get; set; }
        public int? EpochTime { get; set; }
        public string? WeatherText { get; set; }
        public int? WeatherIcon { get; set; }
        public bool? HasPrecipitation { get; set; }
        public string? PrecipitationType { get; set; }
        public bool? IsDayTime { get; set; }
        public TemperatureDto? Temperature { get; set; }

        public string? MobileLink { get; set; }
        public string? Link { get; set; }
    }

    public class TemperatureDto
    {
        public WeatherValueDto? Metric { get; set; }
        public WeatherValueDto? Imperial { get; set; }
    }

    public class WeatherValueDto
    {
        public double? Value { get; set; }
        public string? Unit { get; set; }
        public int? UnitType { get; set; }
    }

}



