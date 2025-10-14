using JekShop.Core.Domain;
using JekShop.Core.Dto;
using JekShop.Data;
using Microsoft.Extensions.Hosting;
using JekShop.Core.ServiceInterface;

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
                if (!Directory.Exists(_webHost.ContentRootPath + "\\multipleFileUpload\\"))
                {
                    Directory.CreateDirectory(_webHost.ContentRootPath + "\\multipleFileUpload\\");
                }

                foreach (var file in dto.Files)
                {
                    // muutuja string uploadFolder ja siina laetakse failid
                    string uploadFolder = Path.Combine(_webHost.ContentRootPath, "multipleFileUpload");

                    //muutuja string uniqueFileName ja siin genereeritakse uus Guid ja lisatakse see faili ette
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.Name;

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

        // Salvestab faile andmebaasi
        public void UploadFilesToDatabase(KindergartenDto dto, Kindergarten domain)
        {
            if (dto.Files != null && dto.Files.Count > 0)
            {
                foreach (var file in dto.Files)
                {
                    using (var target = new MemoryStream())
                    {
                        FileToDatabase files = new FileToDatabase()
                        {
                            Id = Guid.NewGuid(),
                            ImageTitle = file.FileName,
                            KindergartenId = domain.Id
                        };

                        file.CopyTo(target);
                        files.ImageData = target.ToArray();

                        _context.FileToDatabase.Add(files);
                    }
                }
            }
        }

        // Eemaldab ühe pildi andmebaasist
        public async Task<FileToDatabase> RemoveImageFromDatabase(FileToDatabaseDto dto)
        {
            var imageId = await _context.FileToDatabase
                .FirstOrDefaultAsync(x => x.Id == dto.Id);

            if (imageId != null)
            {
                _context.FileToDatabase.Remove(imageId);
                await _context.SaveChangesAsync();

                return imageId;
            }

            return null;
        }

        // Eemaldab kõik faile andmebaasist
        public async Task<FileToDatabase> RemoveImagesFromDatabase(FileToDatabaseDto[] dtos)
        {
            foreach (var dto in dtos)
            {
                var imageId = await _context.FileToDatabase
                    .FirstOrDefaultAsync(x => x.Id == dto.Id);

                if (imageId != null)
                {
                    _context.FileToDatabase.Remove(imageId);
                    await _context.SaveChangesAsync();
                }
            }
            return null;
        }   
    }
}



    }
}
