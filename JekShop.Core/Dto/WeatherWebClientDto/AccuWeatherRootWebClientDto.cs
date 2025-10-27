using System;
using System.Collections.Generic;

namespace JekShop.Core.Dto.WeatherWebClientDto
{
    public class AccuWeatherRootWebClientDto
    {
        public Headline? Headline { get; set; }
        public List<DailyForecast>? DailyForecasts { get; set; }
    }

    public class Headline
    {
        public DateTime? EffectiveDate { get; set; }
        public long? EffectiveEpochDate { get; set; }
        public int? Severity { get; set; }
        public string? Text { get; set; }
        public string? Category { get; set; }
        public DateTime? EndDate { get; set; }
        public long? EndEpochDate { get; set; }
        public string? MobileLink { get; set; }
        public string? Link { get; set; }
    }

    public class DailyForecast
    {
        public DateTime? Date { get; set; }
        public long? EpochDate { get; set; }
        public Temperature? Temperature { get; set; }
        public DayPart? Day { get; set; }
        public DayPart? Night { get; set; }
    }

    public class Temperature
    {
        public UnitValue? Minimum { get; set; }
        public UnitValue? Maximum { get; set; }
    }

    public class UnitValue
    {
        public double? Value { get; set; }
        public string? Unit { get; set; }
        public int? UnitType { get; set; }
    }

    public class DayPart
    {
        public int? Icon { get; set; }
        public string? IconPhrase { get; set; }
        public bool? HasPrecipitation { get; set; }
        public string? PrecipitationType { get; set; }
        public string? PrecipitationIntensity { get; set; }
    }
}
