using JekShop.Core.Domain;
using JekShop.Core.Dto;

namespace JekShop.Core.ServiceInterface
{
    public interface IFileServices
    {
        void FilesToApi(SpaceshipDto dto, Spaceship spaceship);
        void UploadFilesToDatabase(KindergartenDto dto, Kindergarten domain);
        Task<FileToDatabase> RemoveImageFromDatabase(FileToDatabaseDto dto);

        Task<FileToDatabase> RemoveImagesFromDatabase(FileToDatabaseDto[] dtos);


    }
}
