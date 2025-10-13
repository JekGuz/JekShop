using System.Data;

namespace JekShop.Core.Dto
{
    public class AccuLocationRootDto
    {
        public DataSetDateTime? LocalObservatioDateTime { get; set; }
        public int? EpochTime { get; set; }
        public string? WeatherText { get; set; }
        public int? WeatherIcon { get; set; }
        public bool? HasPrecipitation { get; set; }
        public string? PrecipitationType { get; set; }
        public bool? IsDayTime { get; set; }
        public AccuLocationTemperatureDto? Temperature { get; set; }
        public AccuLocationImperialDto? Imperial { get; set; }
        public string? MobileLink { get; set; }
        public string? Link { get; set; }


    }
}
