using System.Net.Http.Headers;
using System.Text.Json;
using JekShop.Core.Dto;
using JekShop.Core.ServiceInterface;
using Microsoft.Extensions.Configuration;

namespace JekShop.ApplicationServices.Services
{
    public class OpenWeatherServices : IOpenWeatherServices
    {
        private readonly string _apiKey;
        private readonly string _baseUrl;
        private readonly string _defaultUnits;
        private readonly string _defaultLang;

        private static readonly JsonSerializerOptions JsonOpts = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public OpenWeatherServices(IConfiguration cfg)
        {
            // берём из appsettings.json
            _apiKey = cfg["OpenWeather:ApiKey"]
                      ?? Environment.GetEnvironmentVariable("OPENWEATHER_API_KEY")
                      ?? throw new InvalidOperationException("OpenWeather API key is missing. Put it into appsettings.json (OpenWeather:ApiKey) or set env OPENWEATHER_API_KEY.");

            _baseUrl = cfg["OpenWeather:BaseUrl"] ?? "https://api.openweathermap.org/data/2.5";
            _defaultUnits = cfg["OpenWeather:DefaultUnits"] ?? "metric";
            _defaultLang = cfg["OpenWeather:DefaultLang"] ?? "et";
        }

        public async Task<OpenWeatherDto.Rootobject?> GetCurrentByCityAsync(
            string city, string? units = null, string? lang = null, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(city)) return null;

            var u = units ?? _defaultUnits;
            var l = lang ?? _defaultLang;

            // /weather?q=Tallinn&appid=KEY&units=metric&lang=et
            var url = $"{_baseUrl}/weather?q={Uri.EscapeDataString(city)}&appid={_apiKey}&units={u}&lang={l}";

            using var http = new HttpClient();
            http.DefaultRequestHeaders.Accept.Clear();
            http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var resp = await http.GetAsync(url, ct);
            resp.EnsureSuccessStatusCode();

            var json = await resp.Content.ReadAsStringAsync(ct);
            return JsonSerializer.Deserialize<OpenWeatherDto.Rootobject>(json, JsonOpts);
        }
    }
}
