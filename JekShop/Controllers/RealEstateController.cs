using System.Security.AccessControl;
using JekShop.ApplicationServices.Services;
using JekShop.Core.Domain;
using JekShop.Core.Dto;
using JekShop.Core.ServiceInterface;
using JekShop.Data;
using JekShop.Data.Migrations;
using JekShop.Models.RealEstate;
using JekShop.Models.Spaceships;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace JekShop.Controllers
{
    public class RealEstateController : Controller
    {
        private readonly JekShopContext _context;
        private readonly IRealEstateServices _RealEstateServices;
        public RealEstateController
            (
                JekShopContext context,
                IRealEstateServices realEstateServices
            )
        {
            _context = context;
            _RealEstateServices = realEstateServices;
        }

        public IActionResult Index()
        {
            var result = _context.RealEstates
                .Select(x => new RealEstateIndexModel
                {
                    Id = x.Id,
                    Area = x.Area,
                    Location = x.Location,
                    RoomNumber = x.RoomNumber,
                    BuildingType = x.BuildingType,
                });


            return View(result);
        }
        [HttpGet]
        public IActionResult Create()
        {
            RealEstateCreateUpdateVeiwModel result = new();
            return View("CreateUpdate", result);
        }
        [HttpPost]
        public async Task<IActionResult> Create(RealEstateCreateUpdateVeiwModel vm)
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

            };

            var result = await _kindergartensServices.Create(dto);

            if (result == null)
            {
                return RedirectToAction(nameof(Index));
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
            var vm = new KindergartenCreateUpdateVeiwModel();

            vm.Id = update.Id;
            vm.GroupName = update.GroupName;
            vm.ChildrenCount = update.ChildrenCount;
            vm.KindergartenName = update.KindergartenName;
            vm.TeacherName = update.TeacherName;
            vm.CreateAt = update.CreateAt;
            vm.UpdateAt = update.UpdateAt;


            return View("CreateUpdate", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Update(KindergartenCreateUpdateVeiwModel vm)
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
    }

}