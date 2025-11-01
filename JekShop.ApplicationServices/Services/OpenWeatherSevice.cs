using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using JekShop.Core.Dto;
using JekShop.Core.ServiceInterface;
using Microsoft.Extensions.Configuration;

namespace JekShop.ApplicationServices.Services
{
    public class OpenWeatherService : IOpenWeatherService
    {
        private readonly HttpClient _http;
        private readonly string _apiKey;
        private readonly string _baseUrl;
        private readonly string _defaultUnits;

        private static readonly JsonSerializerOptions JsonOpts = new()
        {
            PropertyNameCaseInsensitive = true,
            NumberHandling = JsonNumberHandling.AllowReadingFromString
        };

        // ВАЖНО: typed client требует HttpClient в конструкторе
        public OpenWeatherService(HttpClient http, IConfiguration cfg)
        {
            _http = http;
            _apiKey = cfg["OpenWeather:ApiKey"]
                      ?? Environment.GetEnvironmentVariable("OPENWEATHER_API_KEY")
                      ?? throw new InvalidOperationException("OpenWeather API key is missing.");
            _baseUrl = cfg["OpenWeather:BaseUrl"] ?? "https://api.openweathermap.org/data/2.5";
            _defaultUnits = cfg["OpenWeather:DefaultUnits"] ?? "metric";

            // Можно настроить заголовки один раз
            _http.DefaultRequestHeaders.Accept.Clear();
            _http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<OpenWeatherDto.Rootobject?> GetCurrentByCityAsync(
            string city, string? units = null, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(city)) return null;

            var u = units ?? _defaultUnits;
            var url = $"{_baseUrl}/weather?q={Uri.EscapeDataString(city)}&appid={_apiKey}&units={u}";

            var resp = await _http.GetAsync(url, ct);
            if (!resp.IsSuccessStatusCode) return null;

            var json = await resp.Content.ReadAsStringAsync(ct);
            return JsonSerializer.Deserialize<OpenWeatherDto.Rootobject>(json, JsonOpts);
        }
    public async Task<OpenWeatherForecastDto?> GetForecastByCityAsync(
    string city, string? units = null, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(city)) return null;
            var u = units ?? _defaultUnits;
            var url = $"{_baseUrl}/forecast?q={Uri.EscapeDataString(city)}&appid={_apiKey}&units={u}";
            var resp = await _http.GetAsync(url, ct);
            if (!resp.IsSuccessStatusCode) return null;
            var json = await resp.Content.ReadAsStringAsync(ct);
            return JsonSerializer.Deserialize<OpenWeatherForecastDto>(json, JsonOpts);
        }
    }
}
