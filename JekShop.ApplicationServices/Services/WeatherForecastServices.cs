using System.Net;
using System.Text.Json;
using JekShop.Core.Dto;
using JekShop.Core.Dto.WeatherWebClientDto;
using JekShop.Core.ServiceInterface;
using JekShop.Core.ServiceInterface;
using Nancy.Json;


namespace JekShop.ApplicationServices.Services
{
    public class WeatherForecastServices : IWeatherForecastServices
    {

        public async Task<AccuLocationWeatherResultDto> AccuWeatherResult(AccuLocationWeatherResultDto dto)
        {
            //https://developer.accuweather.com/core-weather/text-search?lang=shell#city-search

            string accuApiKey = "zpka_0c86f3fafa9147e58813fa06b647f221_9b9fd9d9";
            string baseUrl = "http://dataservice.accuweather.com/forecasts/v1/daily/1day/";

            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(baseUrl);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                //127964
                var response = await httpClient.GetAsync($"{127964}?apikey={accuApiKey}&details=true");
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var weatherData = JsonSerializer.Deserialize<AccuLocationRootDto>(jsonResponse);
                    //return weatherData;

                    dto.EndDate = weatherData.Headline.EndDate;
                    dto.Text = weatherData.Headline.Text;
                    dto.TempMaxValue = weatherData.DailyForecasts[0].Temperature.Maximum.Value;
                }
                else
                {
                    // Handle error response
                    throw new Exception("Error fetching weather data");
                }

                return dto;
            }
        }


        public async Task<AccuLocationWeatherResultDto> AccuWeatherResultWebClient(AccuLocationWeatherResultDto dto)
        {
            string accuApiKey = "zpka_b5fc90a238324725870040a2c98952f7_6eaf258f";
            string url = $"http://dataservice.accuweather.com/locations/v1/cities/search?apikey={accuApiKey}&q={dto.CityName}";

            using (WebClient client = new WebClient())
            {
                string json = client.DownloadString(url);
                //127964
                List<AccuLocationRootWebClientDto> accuResult = new JavaScriptSerializer()
                    .Deserialize<List<AccuLocationRootWebClientDto>>(json);

                dto.CityName = accuResult[0].LocalizedName;
                dto.CityCode = accuResult[0].Key;
            }

            string urlWeather = $"https://dataservice.accuweather.com/forecasts/v1/daily/1day/{dto.CityCode}?apikey={accuApiKey}&metric=true";

            using (WebClient client = new WebClient())
            {
                string json = client.DownloadString(urlWeather);
                AccuWeatherRootWebClientDto weatherRootDto = new JavaScriptSerializer()
                    .Deserialize<AccuWeatherRootWebClientDto>(json);

                dto.EffectiveDate = weatherRootDto.Headline.EffectiveDate;
                dto.EffectiveEpochDate = weatherRootDto.Headline.EffectiveEpochDate;
                dto.Severity = weatherRootDto.Headline.Severity;
                dto.Text = weatherRootDto.Headline.Text;
                dto.Category = weatherRootDto.Headline.Category;
                dto.EndDate = weatherRootDto.Headline.EndDate;
                dto.EndEpochDate = weatherRootDto.Headline.EndEpochDate;

                dto.MobileLink = weatherRootDto.Headline.MobileLink;
                dto.Link = weatherRootDto.Headline.Link;

                //var dailyForecasts = weatherRootDto.DailyForecasts[0];

                dto.DailyForecastsDate = weatherRootDto.DailyForecasts[0].Date;
                dto.DailyForecastsEpochDate = weatherRootDto.DailyForecasts[0].EpochDate;

                dto.TempMinValue = weatherRootDto.DailyForecasts[0].Temperature.Minimum.Value;
                dto.TempMinUnit = weatherRootDto.DailyForecasts[0].Temperature.Minimum.Unit;
                dto.TempMinUnitType = weatherRootDto.DailyForecasts[0].Temperature.Minimum.UnitType;

                dto.TempMaxValue = weatherRootDto.DailyForecasts[0].Temperature.Maximum.Value;
                dto.TempMaxUnit = weatherRootDto.DailyForecasts[0].Temperature.Maximum.Unit;
                dto.TempMaxUnitType = weatherRootDto.DailyForecasts[0].Temperature.Maximum.UnitType;

                dto.DayIcon = weatherRootDto.DailyForecasts[0].Day.Icon;
                dto.DayIconPhrase = weatherRootDto.DailyForecasts[0].Day.IconPhrase;
                dto.DayHasPrecipitation = weatherRootDto.DailyForecasts[0].Day.HasPrecipitation;
                dto.DayPrecipitationType = weatherRootDto.DailyForecasts[0].Day.PrecipitationType;
                dto.DayPrecipitationIntensity = weatherRootDto.DailyForecasts[0].Day.PrecipitationIntensity;

                dto.NightIcon = weatherRootDto.DailyForecasts[0].Night.Icon;
                dto.NightIconPhrase = weatherRootDto.DailyForecasts[0].Night.IconPhrase;
                dto.NightHasPrecipitation = weatherRootDto.DailyForecasts[0].Night.HasPrecipitation;
                dto.NightPrecipitationType = weatherRootDto.DailyForecasts[0].Night.PrecipitationType;
                dto.NightPrecipitationIntensity = weatherRootDto.DailyForecasts[0].Night.PrecipitationIntensity;
            }

            return dto;
        }
    }
}