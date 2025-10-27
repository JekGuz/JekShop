using System.Text.Json;
using JekShop.Core.Dto;
using JekShop.Core.ServiceInterface;

namespace JekShop.ApplicationServices.Services
{
    public class CocktailService : ICocktailServices
    {
        private readonly HttpClient _http;

        public CocktailService(HttpClient http)
        {
            _http = http;
            _http.BaseAddress = new Uri("https://www.thecocktaildb.com/");
        }

        public async Task<CocktailDto.Rootobject?> SearchAsync(string name)
        {
            var url = $"api/json/v1/1/search.php?s={Uri.EscapeDataString(name ?? string.Empty)}";
            var resp = await _http.GetAsync(url);
            resp.EnsureSuccessStatusCode();

            var json = await resp.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<CocktailDto.Rootobject>(
                json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );
        }
    }
}