using JekShop.Core.Dto;
using JekShop.Core.ServiceInterface;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace JekShop.ApplicationServices.Services
{
    public class ChuckJokeService : IChuckJokeService
    {
        private readonly HttpClient _httpClient;

        public ChuckJokeService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ChuckJokeDto> GetRandomAsync()
        {
            string apiUrl = "https://api.chucknorris.io/jokes/random";

            // Делаем запрос к API
            var response = await _httpClient.GetAsync(apiUrl);
            response.EnsureSuccessStatusCode();

            // Читаем ответ
            var json = await response.Content.ReadAsStringAsync();

            // Десериализация
            var joke = JsonSerializer.Deserialize<ChuckJokeDto>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return joke;
        }
    }
}
