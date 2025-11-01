using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization; // добавоено для использования
using JekShop.Core.Dto;
using JekShop.Core.ServiceInterface;
using Microsoft.Extensions.Configuration;

namespace JekShop.ApplicationServices.Services
{
    public class OpenWeatherService : IOpenWeatherService
    {
        private readonly string _apiKey;
        private readonly string _baseUrl;
        private readonly string _defaultUnits;

        // ✅ обновлённые настройки десериализации
        private static readonly JsonSerializerOptions JsonOpts = new()
        {
            PropertyNameCaseInsensitive = true,
            NumberHandling = JsonNumberHandling.AllowReadingFromString // разрешаем числа в виде строк
        };

        public OpenWeatherService(IConfiguration cfg)
        {
            _apiKey = cfg["OpenWeather:ApiKey"]
                      ?? Environment.GetEnvironmentVariable("OPENWEATHER_API_KEY")
                      ?? throw new InvalidOperationException("OpenWeather API key is missing.");
            _baseUrl = cfg["OpenWeather:BaseUrl"] ?? "https://api.openweathermap.org/data/2.5";
            _defaultUnits = cfg["OpenWeather:DefaultUnits"] ?? "metric";
        }

        public async Task<OpenWeatherDto.Rootobject?> GetCurrentByCityAsync(
            string city, string? units = null, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(city)) return null;

            var u = units ?? _defaultUnits; // °C по умолчанию
            var url = $"{_baseUrl}/weather?q={Uri.EscapeDataString(city)}&appid={_apiKey}&units={u}";

            using var http = new HttpClient();
            http.DefaultRequestHeaders.Accept.Clear();
            http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var resp = await http.GetAsync(url, ct);
            if (!resp.IsSuccessStatusCode)
            {
                // можно залогировать ошибку, чтобы не падало при плохом ответе
                return null;
            }

            var json = await resp.Content.ReadAsStringAsync(ct);
            return JsonSerializer.Deserialize<OpenWeatherDto.Rootobject>(json, JsonOpts);
        }
    }
}
