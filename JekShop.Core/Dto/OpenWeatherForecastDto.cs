namespace JekShop.Core.Dto
{
    public class OpenWeatherForecastDto
    {
        public string? cod { get; set; }
        public int? cnt { get; set; }
        public List<Item>? list { get; set; }
        public City? city { get; set; }

        public class City { public string? name { get; set; } public string? country { get; set; } }
        public class Item
        {
            public long? dt { get; set; }
            public Main? main { get; set; }
            public List<Weather>? weather { get; set; }
        }
        public class Main
        {
            public double? temp { get; set; }
            public double? temp_min { get; set; }
            public double? temp_max { get; set; }
            public int? humidity { get; set; }
        }
        public class Weather
        {
            public int? id { get; set; }
            public string? main { get; set; }
            public string? description { get; set; }
            public string? icon { get; set; }
        }
    }
}
