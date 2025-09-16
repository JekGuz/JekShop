using JekShop.Core.Domain;
using JekShop.Core.Dto;

namespace JekShop.Core.ServiceInterface
{
    public interface IFileServices
    {
        void FileToApi(SpaceshipDto dto, Spaceship spaceship);
    }
}
