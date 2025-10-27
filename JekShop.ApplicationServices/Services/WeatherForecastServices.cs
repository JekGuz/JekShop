using System.Net.Http.Headers;
using System.Text.Json;
using JekShop.Core.Dto;
using JekShop.Core.Dto.WeatherWebClientDto;
using JekShop.Core.ServiceInterface;
using Microsoft.Extensions.Configuration;

namespace JekShop.ApplicationServices.Services
{
    public class WeatherForecastServices : IWeatherForecastServices
    {
        private readonly string _accuApiKey;

        private static readonly JsonSerializerOptions JsonOpts = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public WeatherForecastServices(IConfiguration cfg)
        {
            // приоритет: appsettings.json -> переменная окружения -> исключение
            _accuApiKey =
                cfg["AccuWeather:ApiKey"]
                ?? Environment.GetEnvironmentVariable("ACCUWEATHER_API_KEY")
                ?? throw new InvalidOperationException("AccuWeather API key is missing. Put it into appsettings.json (AccuWeather:ApiKey) or set env ACCUWEATHER_API_KEY.");
        }

        /// Короткий вариант (по фиксированному коду города)
        public async Task<AccuLocationWeatherResultDto> AccuWeatherResult(AccuLocationWeatherResultDto dto)
        {
            const string baseUrl = "http://dataservice.accuweather.com/forecasts/v1/daily/1day/";

            using var httpClient = new HttpClient { BaseAddress = new Uri(baseUrl) };
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            // 127964 — пример: Таллин
            var response = await httpClient.GetAsync($"127964?apikey={_accuApiKey}&details=true&metric=true");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<AccuLocationRootDto>(json, JsonOpts);

            if (data?.Headline != null && data.DailyForecasts?.Length > 0)
            {
                dto.EndDate = data.Headline.EndDate;
                dto.Text = data.Headline.Text;
                dto.TempMaxValue = data.DailyForecasts[0].Temperature?.Maximum?.Value;
            }

            return dto;
        }

        /// Полный вариант через WebClient DTO (по имени города)
        public async Task<AccuLocationWeatherResultDto> AccuWeatherResultWebClient(AccuLocationWeatherResultDto dto)
        {
            using var http = new HttpClient();

            // 1) поиск города
            var urlSearch =
                $"http://dataservice.accuweather.com/locations/v1/cities/search?apikey={_accuApiKey}&q={Uri.EscapeDataString(dto.CityName ?? string.Empty)}";
            var jsonSearch = await http.GetStringAsync(urlSearch);
            var locations = JsonSerializer.Deserialize<List<AccuLocationRootWebClientDto>>(jsonSearch, JsonOpts);

            if (locations == null || locations.Count == 0)
                throw new Exception("City not found from AccuWeather.");

            dto.CityName = locations[0].LocalizedName;
            dto.CityCode = locations[0].Key;

            // 2) прогноз на 1 день
            var urlWeather =
                $"https://dataservice.accuweather.com/forecasts/v1/daily/1day/{dto.CityCode}?apikey={_accuApiKey}&metric=true&details=true";
            var jsonWeather = await http.GetStringAsync(urlWeather);
            var weather = JsonSerializer.Deserialize<AccuWeatherRootWebClientDto>(jsonWeather, JsonOpts);

            // headline
            dto.EffectiveDate = weather?.Headline?.EffectiveDate;
            dto.EffectiveEpochDate = weather?.Headline?.EffectiveEpochDate;
            dto.Severity = weather?.Headline?.Severity;
            dto.Text = weather?.Headline?.Text;
            dto.Category = weather?.Headline?.Category;
            dto.EndDate = weather?.Headline?.EndDate;
            dto.EndEpochDate = weather?.Headline?.EndEpochDate;
            dto.MobileLink = weather?.Headline?.MobileLink;
            dto.Link = weather?.Headline?.Link;

            // первый день прогноза
            var d = weather?.DailyForecasts != null && weather.DailyForecasts.Count > 0
                ? weather.DailyForecasts[0]
                : null;

            if (d != null)
            {
                dto.DailyForecastsDate = d.Date;
                dto.DailyForecastsEpochDate = d.EpochDate;

                dto.TempMinValue = d.Temperature?.Minimum?.Value;
                dto.TempMinUnit = d.Temperature?.Minimum?.Unit;
                dto.TempMinUnitType = d.Temperature?.Minimum?.UnitType;

                dto.TempMaxValue = d.Temperature?.Maximum?.Value;
                dto.TempMaxUnit = d.Temperature?.Maximum?.Unit;
                dto.TempMaxUnitType = d.Temperature?.Maximum?.UnitType;

                dto.DayIcon = d.Day?.Icon;
                dto.DayIconPhrase = d.Day?.IconPhrase;
                dto.DayHasPrecipitation = d.Day?.HasPrecipitation;
                dto.DayPrecipitationType = d.Day?.PrecipitationType;
                dto.DayPrecipitationIntensity = d.Day?.PrecipitationIntensity;

                dto.NightIcon = d.Night?.Icon;
                dto.NightIconPhrase = d.Night?.IconPhrase;
                dto.NightHasPrecipitation = d.Night?.HasPrecipitation;
                dto.NightPrecipitationType = d.Night?.PrecipitationType;
                dto.NightPrecipitationIntensity = d.Night?.PrecipitationIntensity;
            }

            return dto;
        }
    }
}
