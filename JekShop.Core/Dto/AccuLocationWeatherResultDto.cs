public class AccuLocationWeatherResultDto
{
    public string? CityName { get; set; }
    public string? CityCode { get; set; }

    // Заголовок
    public DateTime? EffectiveDate { get; set; }
    public long? EffectiveEpochDate { get; set; }   // ← long?
    public int? Severity { get; set; }
    public string? Text { get; set; }
    public string? Category { get; set; }
    public DateTime? EndDate { get; set; }
    public long? EndEpochDate { get; set; }         // ← long?

    // Дата прогноза
    public DateTime? DailyForecastsDate { get; set; }
    public long? DailyForecastsEpochDate { get; set; } // ← long?

    // Температуры
    public double? TempMinValue { get; set; }
    public string? TempMinUnit { get; set; }
    public int? TempMinUnitType { get; set; }
    public double? TempMaxValue { get; set; }
    public string? TempMaxUnit { get; set; }
    public int? TempMaxUnitType { get; set; }

    // День
    public int? DayIcon { get; set; }
    public string? DayIconPhrase { get; set; }
    public bool? DayHasPrecipitation { get; set; }
    public string? DayPrecipitationType { get; set; }
    public string? DayPrecipitationIntensity { get; set; }

    // Ночь
    public int? NightIcon { get; set; }
    public string? NightIconPhrase { get; set; }
    public bool? NightHasPrecipitation { get; set; }
    public string? NightPrecipitationType { get; set; }
    public string? NightPrecipitationIntensity { get; set; }

    public string? MobileLink { get; set; }
    public string? Link { get; set; }
}
