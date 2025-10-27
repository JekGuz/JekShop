using JekShop.Core.Dto;

namespace JekShop.Core.ServiceInterface
{
    public interface ICocktailServices
    {
        Task<CocktailDto.Rootobject?> SearchAsync(string name);
    }
}