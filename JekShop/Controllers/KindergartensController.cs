using System.IO; // напоминание: для MemoryStream/Path
using System.Security.AccessControl;
using JekShop.ApplicationServices.Services;
using JekShop.Core.Domain;
using JekShop.Core.Dto;
using JekShop.Core.ServiceInterface;
using JekShop.Data;
using JekShop.Data.Migrations;
using JekShop.Models.Kindergartens;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using JekShop.ApplicationServices.Services;

namespace JekShop.Controllers
{
    public class KindergartensController : Controller
    {
        private readonly JekShopContext _context;
        private readonly IKindergartenServices _kindergartensServices;
        private readonly IFileServices _fileServices;

        public KindergartensController
            (
                JekShopContext context,
                IKindergartenServices kindergatensServices,
                IFileServices fileServices
            )
        {
            _context = context;
            _kindergartensServices = kindergatensServices;
            _fileServices = fileServices;
        }

        public IActionResult Index()
        {
            var result = _context.Kindergartens
                .Select(x => new KindergartensIndexViewModel
                {
                    Id = x.Id,
                    GroupName = x.GroupName,
                    ChildrenCount = x.ChildrenCount,
                    KindergartenName = x.KindergartenName,
                    TeacherName = x.TeacherName,
                });

            return View(result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            KindergartenCreateUpdateVeiwModel result = new();
            return View("CreateUpdate", result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(KindergartenCreateUpdateVeiwModel vm)
        {
            // 1) Сначала подготавливаем файлы
            var imageDtos = new List<FileToDatabaseDto>();
            if (vm.Files != null && vm.Files.Any())
            {
                foreach (var file in vm.Files)
                {
                    using var ms = new MemoryStream();
                    await file.CopyToAsync(ms);
                    imageDtos.Add(new FileToDatabaseDto
                    {
                        Id = Guid.NewGuid(),
                        ImageTitle = Path.GetFileName(file.FileName),
                        ImageData = ms.ToArray(),
                        KindergartenId = vm.Id
                    });
                }
            }

            // 2) Создаём DTO для сохранения
            var dto = new KindergartenDto()
            {
                Id = vm.Id,
                GroupName = vm.GroupName,
                ChildrenCount = vm.ChildrenCount,
                KindergartenName = vm.KindergartenName,
                TeacherName = vm.TeacherName,
                Files = vm.Files,
                Image = imageDtos.ToArray(),
                CreateAt = vm.CreateAt,
                UpdateAt = vm.UpdateAt,
            };

            // 3) Сохраняем садик в базу через сервис
            var result = await _kindergartensServices.Create(dto);

            // 4) Сразу после — добавляем реальные файлы в таблицу
            if (result != null && vm.Files != null && vm.Files.Any())
            {
                var entities = new List<FileToDatabase>();
                foreach (var file in vm.Files)
                {
                    using var ms = new MemoryStream();
                    await file.CopyToAsync(ms);

                    entities.Add(new FileToDatabase
                    {
                        Id = Guid.NewGuid(),
                        KindergartenId = result.Id,        // важное отличие!
                        ImageTitle = Path.GetFileName(file.FileName),
                        ImageData = ms.ToArray()
                    });
                }
                _context.FileToDatabases.AddRange(entities);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var kindergarten = await _kindergartensServices.DetailAsync(id);
            if (kindergarten == null)
            {
                return NotFound();
            }

            KindergartenImageViewModel[] images = await FilesFromDatabase(id);

            var vm = new KindergartenDeleteViewModel
            {
                Id = kindergarten.Id,
                GroupName = kindergarten.GroupName,
                ChildrenCount = kindergarten.ChildrenCount,
                KindergartenName = kindergarten.KindergartenName,
                TeacherName = kindergarten.TeacherName,
                CreateAt = kindergarten.CreateAt,
                UpdateAt = kindergarten.UpdateAt
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmation(Guid id)
        {
            var deleted = await _kindergartensServices.Delete(id);
            if (deleted == null)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var update = await _kindergartensServices.DetailAsync(id);
            if (update == null)
            {
                return NotFound();
            }

            var kindergartenImages = await ShowImages(id);

            var vm = new KindergartenCreateUpdateVeiwModel
            {
                Id = update.Id,
                GroupName = update.GroupName,
                ChildrenCount = update.ChildrenCount,
                KindergartenName = update.KindergartenName,
                TeacherName = update.TeacherName,
                CreateAt = update.CreateAt,
                UpdateAt = update.UpdateAt
            };

            //Images может быть null -> подстрахуемся
            vm.Images ??= new List<KindergartenImageViewModel>();
            vm.Images.AddRange(kindergartenImages);

            return View("CreateUpdate", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Update(KindergartenCreateUpdateVeiwModel vm)
        {
            // Не теряем старые картинки и добавляем новые из Files
            var imageDtos = new List<FileToDatabaseDto>();

            if (vm.Files != null && vm.Files.Any())
            {
                foreach (var file in vm.Files)
                {
                    using var ms = new MemoryStream();
                    await file.CopyToAsync(ms);
                    imageDtos.Add(new FileToDatabaseDto
                    {
                        Id = Guid.NewGuid(),
                        ImageTitle = Path.GetFileName(file.FileName),
                        ImageData = ms.ToArray(),
                        KindergartenId = vm.Id
                    });
                }
            }

            if (vm.Images != null && vm.Images.Any())
            {
                imageDtos.AddRange(vm.Images.Select(x => new FileToDatabaseDto
                {
                    Id = x.Id,
                    ImageTitle = x.ImageTitle,
                    ImageData = x.ImageData,
                    KindergartenId = x.KindergartenId
                }));
            }

            // 2️⃣ Создаём DTO для обновления
            var dto = new KindergartenDto()
            {
                Id = vm.Id,
                GroupName = vm.GroupName,
                ChildrenCount = vm.ChildrenCount,
                KindergartenName = vm.KindergartenName,
                TeacherName = vm.TeacherName,
                CreateAt = vm.CreateAt,
                UpdateAt = vm.UpdateAt,
                Files = vm.Files,
                Image = imageDtos.ToArray()
            };

            // 3️⃣ Обновляем через сервис
            var result = await _kindergartensServices.Update(dto);

            // 4️⃣ Добавляем новые файлы в таблицу FileToDatabases
            if (result != null && vm.Files != null && vm.Files.Any())
            {
                var entities = new List<FileToDatabase>();
                foreach (var file in vm.Files)
                {
                    using var ms = new MemoryStream();
                    await file.CopyToAsync(ms);

                    entities.Add(new FileToDatabase
                    {
                        Id = Guid.NewGuid(),
                        KindergartenId = vm.Id,                // У апдейта ID уже известен
                        ImageTitle = Path.GetFileName(file.FileName),
                        ImageData = ms.ToArray()
                    });
                }
                _context.FileToDatabases.AddRange(entities);
                await _context.SaveChangesAsync();             // Сохраняем в БД
            }

            // Возвращаемся на список
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var kindergarten = await _kindergartensServices.DetailAsync(id);
            if (kindergarten == null)
            {
                return NotFound();
            }

            var photo = await ShowImages(id);

            var vm = new KindergartenDeleteViewModel
            {
                Id = kindergarten.Id,
                GroupName = kindergarten.GroupName,
                ChildrenCount = kindergarten.ChildrenCount,
                KindergartenName = kindergarten.KindergartenName,
                TeacherName = kindergarten.TeacherName,
                CreateAt = kindergarten.CreateAt,
                UpdateAt = kindergarten.UpdateAt
            };

            vm.Images ??= new List<KindergartenImageViewModel>(); // защита от null
            vm.Images.AddRange(photo);

            return View(vm);
        }

        public async Task<KindergartenImageViewModel[]> ShowImages(Guid id)
        {
            var images = await _context.FileToDatabases
                .Where(x => x.KindergartenId == id)
                .Select(y => new KindergartenImageViewModel
                {
                    KindergartenId = y.KindergartenId,
                    Id = y.Id,
                    ImageData = y.ImageData,
                    ImageTitle = y.ImageTitle,
                    Image = $"data:image/jpeg;base64,{Convert.ToBase64String(y.ImageData)}"
                }).ToArrayAsync();

            return images;
        }

        // Meetod piltide toomiseks andmebaasist
        public async Task<KindergartenImageViewModel[]> FilesFromDatabase(Guid id)
        {
            var images = await _context.KindergartenFileToDatabase
                .Where(x => x.KindergartenId == id)
                .Select(y => new KindergartenImageViewModel
                {
                    KindergartenId = y.KindergartenId,
                    Id = y.Id,
                    ImageData = y.ImageData,
                    ImageTitle = y.ImageTitle,
                    Image = string.Format("data:image/gif;base64, {0}", Convert.ToBase64String(y.ImageData))
                }).ToArrayAsync();

            return images;
        }

        // Meetod piltide toomiseks andmebaasist
        public async Task<KindergartenImageViewModel[]> FilesFromDatabase(Guid id)
        {
            var images = await _context.FileToDatabase
                .Where(x => x.KindergartenId == id)
                .Select(y => new KindergartenImageViewModel
                {
                    KindergartenId = y.KindergartenId,
                    Id = y.Id,
                    ImageData = y.ImageData,
                    ImageTitle = y.ImageTitle,
                    Image = string.Format("data:image/gif;base64, {0}", Convert.ToBase64String(y.ImageData))
                }).ToArrayAsync();

            return images;
        }

        // Meetod ühe pilte eemaldamiseks andmebaasist
        [HttpPost]
        public async Task<IActionResult> RemoveImage(KindergartenImageViewModel vm)
        {
            var dto = new FileToDatabaseDto()
            {
                Id = vm.Id
            };

            var image = await _fileServices.RemoveImageFromDatabase(dto);

            if (image == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }
    }
}

