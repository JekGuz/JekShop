namespace JekShop.Core.ServiceInterface
{
    public interface IChuckJokeService
    {
        Task<JekShop.Core.Dto.ChuckJokeDto> GetRandomAsync();
    }
}