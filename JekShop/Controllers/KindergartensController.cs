using System.Security.AccessControl;
using JekShop.ApplicationServices.Services;
using JekShop.Core.Domain;
using JekShop.Core.Dto;
using JekShop.Core.ServiceInterface;
using JekShop.Data;
using JekShop.Data.Migrations;
using JekShop.Models.Kindergartens;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace JekShop.Controllers
{
    public class KindergartensController : Controller
    {
        private readonly JekShopContext _context;
        private readonly IKindergartenServices _kindergartensServices;
        public KindergartensController
            (
                JekShopContext context,
                IKindergartenServices kindergatensServices
            )
        {
            _context = context;
            _kindergartensServices = kindergatensServices;
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
            var dto = new KindergartenDto()
            {
                Id = vm.Id,
                GroupName = vm.GroupName,
                ChildrenCount = vm.ChildrenCount,
                KindergartenName = vm.KindergartenName,
                TeacherName = vm.TeacherName,
                Files = vm.Files,
                Image = vm.Images
                    .Select(x => new FileToDatabaseDto
                    {
                        Id = x.Id,
                        ImageTitle = x.ImageTitle,
                        ImageData = x.ImageData,
                        KindergartenId = x.KindergartenId
                    }).ToArray(),


                CreateAt = vm.CreateAt,
                UpdateAt = vm.UpdateAt,

            };

            var result = await _kindergartensServices.Create(dto);

            if (result == null)
            { 
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task <IActionResult> Delete(Guid id)
        {
            var kindergarten = await _kindergartensServices.DetailAsync(id);

            if (kindergarten == null)
            { 
                return NotFound();
            }

            var vm = new KindergartenDeleteViewModel();

            vm.Id = kindergarten.Id;
            vm.GroupName = kindergarten.GroupName;
            vm.ChildrenCount = kindergarten.ChildrenCount;
            vm.KindergartenName = kindergarten.KindergartenName;
            vm.TeacherName = kindergarten.TeacherName;
            vm.CreateAt = kindergarten.CreateAt;
            vm.UpdateAt = kindergarten.UpdateAt;


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

            var KindergartenImage = await ShowImages(id);

            var vm = new KindergartenCreateUpdateVeiwModel();

            vm.Id = update.Id;
            vm.GroupName = update.GroupName;
            vm.ChildrenCount = update.ChildrenCount;
            vm.KindergartenName = update.KindergartenName;
            vm.TeacherName = update.TeacherName;
            vm.CreateAt = update.CreateAt;
            vm.UpdateAt = update.UpdateAt;
            vm.Images.AddRange(KindergartenImage);


            return View("CreateUpdate", vm);
        }

        [HttpPost]
        public async Task<IActionResult>Update(KindergartenCreateUpdateVeiwModel vm)
        {
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
                Image = vm.Images
                    .Select(x => new FileToDatabaseDto
                    {
                        Id = x.Id,
                        ImageTitle = x.ImageTitle,
                        ImageData = x.ImageData,
                        KindergartenId = x.KindergartenId
                    }).ToArray(),

            };

            var result = await _kindergartensServices.Update(dto);

            if (result == null)
            {
                return RedirectToAction(nameof(Index));
            }

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

            var vm = new KindergartenDeleteViewModel();

            vm.Id = kindergarten.Id;
            vm.GroupName = kindergarten.GroupName;
            vm.ChildrenCount = kindergarten.ChildrenCount;
            vm.KindergartenName = kindergarten.KindergartenName;
            vm.TeacherName = kindergarten.TeacherName;
            vm.CreateAt = kindergarten.CreateAt;
            vm.UpdateAt = kindergarten.UpdateAt;
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
                    Image = string.Format("data:image/gif;base64, {0}", Convert.ToBase64String(y.ImageData))
                }).ToArrayAsync();

            return images;
        }
    }

}
