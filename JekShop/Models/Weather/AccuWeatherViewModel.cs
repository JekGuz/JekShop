namespace JekShop.Models.Weather
{
    public class AccuWeatherViewModel
    {
        public string? CityName { get; set; }

        public DateTime? EffectiveDate { get; set; }
        public long? EffectiveEpochDate { get; set; }
        public int? Severity { get; set; }
        public string? Text { get; set; }
        public string? Category { get; set; }
        public DateTime? EndDate { get; set; }
        public long? EndEpochDate { get; set; }

        public DateTime? DailyForecastsDate { get; set; }
        public long? DailyForecastsEpochDate { get; set; }

        public double? TempMinValue { get; set; }
        public string? TempMinUnit { get; set; }
        public int? TempMinUnitType { get; set; }
        public double? TempMaxValue { get; set; }
        public string? TempMaxUnit { get; set; }
        public int? TempMaxUnitType { get; set; }

        public int? DayIcon { get; set; }
        public string? DayIconPhrase { get; set; }
        public bool? DayHasPrecipitation { get; set; }
        public string? DayPrecipitationType { get; set; }
        public string? DayPrecipitationIntensity { get; set; }

        public int? NightIcon { get; set; }
        public string? NightIconPhrase { get; set; }
        public bool? NightHasPrecipitation { get; set; }
        public string? NightPrecipitationType { get; set; }
        public string? NightPrecipitationIntensity { get; set; }

        public string? MobileLink { get; set; }
        public string? Link { get; set; }
    }
}
