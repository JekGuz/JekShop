using System.Security.AccessControl;
using JekShop.ApplicationServices.Services;
using JekShop.Core.Domain;
using JekShop.Core.Dto;
using JekShop.Core.ServiceInterface;
using JekShop.Data;
using JekShop.Data.Migrations;
using JekShop.Models.RealEstate;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JekShop.Controllers
{
    public class RealEstateController : Controller
    {
        private readonly JekShopContext _context;
        private readonly IRealEstateServices _RealEstateServices;
        public RealEstateController
            (
                JekShopContext context,
                IRealEstateServices RealEstateServices
            )
        {
            _context = context;
            _RealEstateServices = RealEstateServices;
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
            var dto = new RealEstateDto()
            {
                Id = vm.Id,
                Area = vm.Area,
                Location = vm.Location,
                RoomNumber = vm.RoomNumber,
                BuildingType = vm.BuildingType,
                Files = vm.Files,
                Image = vm.Images
                    .Select(x => new FileToDatabaseDto
                    {
                        Id = x.Id,
                        ImageTitle = x.ImageTitle,
                        ImageData = x.ImageData,
                        RealEstateId = x.RealEstateId
                    }).ToArray(),

                CreateAt = vm.CreateAt,
                ModifiedAt = vm.ModifiedAt,

            };

            var result = await _RealEstateServices.Create(dto);

            if (result == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var RealEstate = await _RealEstateServices.DetailAsync(id);

            if (RealEstate == null)
            {
                return NotFound();
            }

            var realEstateImages = await ShowImages(id);

            var vm = new RealEstateDeleteViewModel();

            vm.Id = RealEstate.Id;
            vm.Area = RealEstate.Area;
            vm.Location = RealEstate.Location;
            vm.RoomNumber = RealEstate.RoomNumber;
            vm.BuildingType = RealEstate.BuildingType;
            vm.CreateAt = RealEstate.CreateAt;
            vm.ModifiedAt = RealEstate.ModifiedAt;
            vm.Images.AddRange(realEstateImages);

            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmation(Guid id)
        {
            var deleted = await _RealEstateServices.Delete(id);
            if (deleted == null)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var update = await _RealEstateServices.DetailAsync(id);

            if (update == null)
            {
                return NotFound();
            }

            var realEstateImages = await ShowImages(id);

            var vm = new RealEstateCreateUpdateVeiwModel();

            vm.Id = update.Id;
            vm.Area = update.Area;
            vm.Location = update.Location;
            vm.RoomNumber = update.RoomNumber;
            vm.BuildingType = update.BuildingType;
            vm.CreateAt = update.CreateAt;
            vm.ModifiedAt = update.ModifiedAt;
            vm.Images.AddRange(realEstateImages);


            return View("CreateUpdate", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Update(RealEstateCreateUpdateVeiwModel vm)
        {
            var dto = new RealEstateDto()
            {
                Id = vm.Id,
                Area = vm.Area,
                Location = vm.Location,
                RoomNumber = vm.RoomNumber,
                BuildingType = vm.BuildingType,
                CreateAt = vm.CreateAt,
                ModifiedAt = vm.ModifiedAt,
                Files = vm.Files,
                Image = vm.Images
                    .Select(x => new FileToDatabaseDto
                    {
                        Id = x.Id,
                        ImageTitle = x.ImageTitle,
                        ImageData = x.ImageData,
                        RealEstateId = x.RealEstateId
                    }).ToArray(),

            };

            var result = await _RealEstateServices.Update(dto);

            if (result == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var RealEstate = await _RealEstateServices.DetailAsync(id);

            if (RealEstate == null)
            {
                return NotFound();
            }

            var photo = await ShowImages(id);

            var vm = new RealEstateDetailsViewModel();

            vm.Id = RealEstate.Id;
            vm.Area = RealEstate.Area;
            vm.Location = RealEstate.Location;
            vm.RoomNumber = RealEstate.RoomNumber;
            vm.BuildingType = RealEstate.BuildingType;
            vm.CreateAt = RealEstate.CreateAt;
            vm.ModifiedAt = RealEstate.ModifiedAt;
            vm.Images.AddRange(photo);

            return View(vm);
        }


        public async Task<RealEstateImageVeiwModel[]> ShowImages(Guid id)
        {
            var images = await _context.FileToDatabases
                .Where(x => x.RealEstateId == id)
                .Select(y => new RealEstateImageVeiwModel
                {
                    RealEstateId = y.RealEstateId,
                    Id = y.Id,
                    ImageData = y.ImageData,
                    ImageTitle = y.ImageTitle,
                    Image = string.Format("data:image/gif;base64, {0}", Convert.ToBase64String(y.ImageData))
                }).ToArrayAsync();

            return images;
        }
    }
}
