using System.Threading;
using System.Threading.Tasks;
using JekShop.Core.Dto;

namespace JekShop.Core.ServiceInterface
{
    public interface IOpenWeatherService
    {
        Task<OpenWeatherDto.Rootobject?> GetCurrentByCityAsync(
            string city,
            string? units = null,           // metric | imperial
            CancellationToken ct = default);
    }
}
