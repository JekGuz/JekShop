using JekShop.Core.Domain;
using JekShop.Core.Dto;
using JekShop.Core.ServiceInterface;
using JekShop.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace JekShop.ApplicationServices.Services
{
    public class FileServices : IFileServices
    {
        private readonly JekShopContext _context;
        private readonly IHostEnvironment _webHost;


        public FileServices
            (
                JekShopContext context,
                IHostEnvironment webHost
            )
        {
            _context = context;
            _webHost = webHost;
        }


        public void FilesToApi(SpaceshipDto dto, Spaceship spaceship)
        {

            if (dto.Files != null && dto.Files.Count > 0)
            {
                if (!Directory.Exists(_webHost.ContentRootPath + "\\wwwroot\\multipleFileUpload\\"))
                {
                    Directory.CreateDirectory(_webHost.ContentRootPath + "\\wwwroot\\multipleFileUpload\\");
                }

                foreach (var file in dto.Files)
                {
                    // muutuja string uploadFolder ja siina laetakse failid
                    string uploadFolder = Path.Combine(_webHost.ContentRootPath, "wwwroot", "multipleFileUpload");

                    //muutuja string uniqueFileName ja siin genereeritakse uus Guid ja lisatakse see faili ette
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                    // muutuja string filePath kombineeritakse ja lisatakse koos kausta unikaalse nimega
                    string filePath = Path.Combine(uploadFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);

                        FileToApi path = new FileToApi
                        {
                            Id = Guid.NewGuid(),
                            ExistingFilePath = uniqueFileName,
                            SpaceshipId = spaceship.Id
                        };

                        _context.FileToApis.AddAsync(path);
                    }
                }
            }
        }

        public async Task<FileToApi?> RemoveImageFromApi(FileToApiDto dto)
        {

            // Meil on vaja leida file andmebaasist läbi id ülesse
            // 1) Находим запись файла в БД
            var imageId = await _context.FileToApis
                .FirstOrDefaultAsync(x => x.Id == dto.Id);  // чтобы найти что в базе данных

            var filePath = _webHost.ContentRootPath + "\\wwwroot\\multipleFileUpload\\"
                + imageId.ExistingFilePath;

            // Kui fail on olemas, siis kustuta ära 
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            // 4) Удаляем запись из базы
            _context.FileToApis.Remove(imageId);
            await _context.SaveChangesAsync();

            return imageId;
        }
        public async Task<List<FileToApi>> RemoveImagesFromAppi(FileToApiDto[] dtos)
        {
            //foreach, milles sees toimub failide kustutamine
            foreach (var dto in dtos)
            {
                var imageId = await _context.FileToApis
                .FirstOrDefaultAsync(x => x.Id == dto.Id);  // чтобы найти что в базе данных

                var filePath = _webHost.ContentRootPath + "\\wwwroot\\multipleFileUpload\\"
                    + imageId.ExistingFilePath;

                // Kui fail on olemas, siis kustuta ära 
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                // 4) Удаляем запись из базы
                _context.FileToApis.Remove(imageId);
                await _context.SaveChangesAsync();
            }

            return null;
        }

        public void UploadFilesToDatabase(RealEstateDto dto, RealEstate domain)
        {
            // tuleb ära kontrollida, kas üks fail või mitu faili on 
            // kui tuleb mitu faili, siis igaks juhuks tuleks kasutada foreach 
            // foreach sees kasutada using-t ja ära mappida 
            // salvestada andmed andmebaasi

            if (dto.Files != null && dto.Files.Count > 0)
            {
                foreach (var file in dto.Files)
                {
                    using (var target = new MemoryStream())
                    {
                        FileToDatabase files = new FileToDatabase
                        {
                            Id = Guid.NewGuid(),
                            ImageTitle = file.FileName,
                            RealEstateId = domain.Id
                        };

                        file.CopyTo(target);
                        files.ImageData = target.ToArray();

                        _context.FileToDatabases.AddAsync(files);
                    }
                }

            }
        }
    }
}
