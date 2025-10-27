using System.Net.Http;
using System.Text.Json;
using JekShop.Core.Dto;
using JekShop.Core.ServiceInterface;

namespace JekShop.ApplicationServices.Services
{
    public class ChuckJokeService : IChuckJokeService
    {
        private static readonly HttpClient _http = new();

        public async Task<ChuckJokeDto> GetRandomAsync()
        {
            var json = await _http.GetStringAsync("https://api.chucknorris.io/jokes/random");
            var dto = JsonSerializer.Deserialize<ChuckJokeDto>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (dto == null) throw new Exception("Failed to get joke from API.");
            return dto;
        }
    }
}