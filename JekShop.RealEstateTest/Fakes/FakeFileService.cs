namespace JekShop.RealEstateTest.Fakes;

using System.Collections.Generic;
using System.Threading.Tasks;
using JekShop.Core.Domain;
using JekShop.Core.Dto;
using JekShop.Core.ServiceInterface;

public sealed class FakeFileServices : IFileServices
{
    public void FilesToApi(SpaceshipDto dto, Spaceship spaceship)
    {
        // В тесте ничего не делаем
    }

    public Task<FileToApi?> RemoveImageFromApi(FileToApiDto dto)
    {
        // Возвращаем null – просто чтобы метод существовал
        return Task.FromResult<FileToApi?>(null);
    }

    public Task<FileToDatabase> RemoveImageFromDatabase(FileToDatabaseDto[] dtos)
    {
        throw new NotImplementedException();
    }

    public Task<List<FileToApi>> RemoveImagesFromAppi(FileToApiDto[] dtos)
    {
        // Пустой список – нам результат не важен
        return Task.FromResult(new List<FileToApi>());
    }

    public void UploadFilesToDatabase(RealEstateDto dto, RealEstate domain)
    {
        // В тесте не загружаем файлы
    }

}
