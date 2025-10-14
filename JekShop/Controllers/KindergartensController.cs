using System.IO;
using JekShop.Core.Domain;
using JekShop.Core.Dto;
using JekShop.Core.ServiceInterface;
using JekShop.Data;
using JekShop.Models.Kindergartens;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JekShop.Controllers
{
    public class KindergartensController : Controller
    {
        private readonly JekShopContext _context;
        private readonly IKindergartenServices _kindergartenServices;
        private readonly IFileServices _fileServices;

        public KindergartensController(
            JekShopContext context,
            IKindergartenServices kindergartenServices,
            IFileServices fileServices)
        {
            _context = context;
            _kindergartenServices = kindergartenServices;
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
                    TeacherName = x.TeacherName
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
            // подготовим изображения (необязательно, но пусть будет)
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

            var dto = new KindergartenDto
            {
                Id = vm.Id,
                GroupName = vm.GroupName,
                ChildrenCount = vm.ChildrenCount,
                KindergartenName = vm.KindergartenName,
                TeacherName = vm.TeacherName,
                Files = vm.Files,
                Image = imageDtos.ToArray(),
                CreateAt = vm.CreateAt,
                UpdateAt = vm.UpdateAt
            };

            var created = await _kindergartenServices.Create(dto);

            // сразу сохраним реальные файлы в таблицу
            if (created != null && vm.Files != null && vm.Files.Any())
            {
                var entities = new List<FileToDatabase>();
                foreach (var file in vm.Files)
                {
                    using var ms = new MemoryStream();
                    await file.CopyToAsync(ms);
                    entities.Add(new FileToDatabase
                    {
                        Id = Guid.NewGuid(),
                        KindergartenId = created.Id,
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
        public async Task<IActionResult> Update(Guid id)
        {
            var update = await _kindergartenServices.DetailAsync(id);
            if (update == null) return NotFound();

            var images = await ShowImages(id);

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

            vm.Images ??= new List<KindergartenImageViewModel>();
            vm.Images.AddRange(images);

            return View("CreateUpdate", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Update(KindergartenCreateUpdateVeiwModel vm)
        {
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

            var dto = new KindergartenDto
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

            var updated = await _kindergartenServices.Update(dto);

            // добавим новые файлы в БД
            if (updated != null && vm.Files != null && vm.Files.Any())
            {
                var entities = new List<FileToDatabase>();
                foreach (var file in vm.Files)
                {
                    using var ms = new MemoryStream();
                    await file.CopyToAsync(ms);
                    entities.Add(new FileToDatabase
                    {
                        Id = Guid.NewGuid(),
                        KindergartenId = vm.Id,
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
        public async Task<IActionResult> Details(Guid id)
        {
            var kindergarten = await _kindergartenServices.DetailAsync(id);
            if (kindergarten == null) return NotFound();

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

            vm.Images ??= new List<KindergartenImageViewModel>();
            vm.Images.AddRange(photo);

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var kindergarten = await _kindergartenServices.DetailAsync(id);
            if (kindergarten == null) return NotFound();

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
            var deleted = await _kindergartenServices.Delete(id);
            if (deleted == null) return NotFound();
            return RedirectToAction(nameof(Index));
        }

        // жёстко фиксируем путь /Kindergartens/RemoveImage
        [AcceptVerbs("GET", "POST")]
        [Route("/Kindergartens/RemoveImage")]  // начальный слэш = абсолютный маршрут
                                               // [ValidateAntiForgeryToken] // можно вернуть позже
        public async Task<IActionResult> RemoveImage([FromForm] Guid imageId)
        {
            if (imageId == Guid.Empty)
                return BadRequest("imageId is empty");

            var image = await _fileServices.RemoveImageFromDatabase(
                new JekShop.Core.Dto.FileToDatabaseDto { Id = imageId });

            if (image == null)
                return NotFound($"Image {imageId} not found");

            if (image.KindergartenId.HasValue)
                return RedirectToAction(nameof(Update), new { id = image.KindergartenId.Value });

            return RedirectToAction(nameof(Index));
        }


        // загрузка изображений для отображения
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
                })
                .ToArrayAsync();

            return images;
        }
    }
}
