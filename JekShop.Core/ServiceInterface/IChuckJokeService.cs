using JekShop.Core.Dto;
using System.Threading.Tasks;

namespace JekShop.Core.ServiceInterface
{
    public interface IChuckJokeService
    {
        Task<ChuckJokeDto> GetRandomAsync();
    }
}
