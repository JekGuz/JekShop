using JekShop.Core.Domain;
using JekShop.Core.Dto;

namespace JekShop.Core.ServiceInterface
{
    public interface IFileServices
    {
        void FilesToApi(SpaceshipDto dto, Spaceship spaceship);
        Task<FileToApi?> RemoveImageFromApi(FileToApiDto dto);
        Task<List<FileToApi>> RemoveImagesFromAppi(FileToApiDto[] dtos);
        void UploadFilesToDatabase(RealEstateDto dto, RealEstate domain);
        Task<FileToDatabase> RemoveImageFromDatabase(FileToDatabaseDto[] dtos);

    }
}
