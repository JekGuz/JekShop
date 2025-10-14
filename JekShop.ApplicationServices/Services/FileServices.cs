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

        public FileServices(JekShopContext context, IHostEnvironment webHost)
        {
            _context = context;
            _webHost = webHost;
        }

        public void FilesToApi(SpaceshipDto dto, Spaceship spaceship)
        {
            if (dto.Files == null || dto.Files.Count == 0) return;

            var folder = Path.Combine(_webHost.ContentRootPath, "multipleFileUpload");
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

            foreach (var file in dto.Files)
            {
                var uniqueFileName = Guid.NewGuid() + "_" + file.Name;
                var filePath = Path.Combine(folder, uniqueFileName);

                using var fileStream = new FileStream(filePath, FileMode.Create);
                file.CopyTo(fileStream);

                _context.FileToApis.Add(new FileToApi
                {
                    Id = Guid.NewGuid(),
                    ExistingFilePath = uniqueFileName,
                    SpaceshipId = spaceship.Id
                });
            }
        }

        public void UploadFilesToDatabase(KindergartenDto dto, Kindergarten domain)
        {
            if (dto?.Files == null || dto.Files.Count == 0) return;

            foreach (var file in dto.Files)
            {
                using var ms = new MemoryStream();
                file.CopyTo(ms);

                var entity = new FileToDatabase
                {
                    Id = Guid.NewGuid(),
                    ImageTitle = file.FileName,
                    KindergartenId = domain.Id,
                    ImageData = ms.ToArray()
                };

                _context.FileToDatabases.Add(entity);
            }
        }

        public async Task<FileToDatabase?> RemoveImageFromDatabase(FileToDatabaseDto dto)
        {
            var entity = await _context.FileToDatabases
                .FirstOrDefaultAsync(x => x.Id == dto.Id);

            if (entity == null) return null;

            _context.FileToDatabases.Remove(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<FileToDatabase?> RemoveImagesFromDatabase(FileToDatabaseDto[] dtos)
        {
            var ids = dtos.Select(d => d.Id).ToList();
            var entities = await _context.FileToDatabases
                .Where(x => ids.Contains(x.Id))
                .ToListAsync();

            if (entities.Count == 0) return null;

            _context.FileToDatabases.RemoveRange(entities);
            await _context.SaveChangesAsync();
            return null;
        }
    }
}
