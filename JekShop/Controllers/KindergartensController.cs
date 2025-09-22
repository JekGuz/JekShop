using System.Security.AccessControl;
using JekShop.ApplicationServices.Services;
using JekShop.Core.Domain;
using JekShop.Core.Dto;
using JekShop.Core.ServiceInterface;
using JekShop.Data;
using JekShop.Models.Kindergartens;
using JekShop.Models.Spaceships;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

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
            var dto = new SpaceshipDto()
            {
                Id = vm.Id,
                Name = vm.Name,
                TypeName = vm.TypeName,
                BuildDate = vm.BuildDate,
                Crew = vm.Crew,
                EnginePower = vm.EnginePower,
                Passengers = vm.Passengers,
                InnerVolume = vm.InnerVolume,
                CreatedAt = vm.CreatedAt,
                ModifiedAt = vm.ModifiedAt,
            };

            var result = await _spaceshipsServices.Create(dto);

            if (result == null)
            { 
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task <IActionResult> Delete(Guid id)
        {
            var spaceship = await _spaceshipsServices.DetailAsync(id);

            if (spaceship == null)
            { 
                return NotFound();
            }

            var vm = new KindergartenDeleteViewModel();

            vm.Id = spaceship.Id;
            vm.Name = spaceship.Name;
            vm.TypeName = spaceship.TypeName;
            vm.BuildDate = spaceship.BuildDate;
            vm.Crew = spaceship.Crew;
            vm.EnginePower = spaceship.EnginePower;
            vm.Passengers = spaceship.Passengers;
            vm.InnerVolume = spaceship.InnerVolume;
            vm.CreatedAt = spaceship.CreatedAt;
            vm.ModifiedAt = spaceship.ModifiedAt;

            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmation(Guid id)
        {
            var deleted = await _spaceshipsServices.Delete(id);
            if (deleted == null)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var update = await _spaceshipsServices.DetailAsync(id);

            if (update == null)
            {
                return NotFound();
            }
            var vm = new KindergartenCreateUpdateVeiwModel();

            vm.Id = update.Id;
            vm.Name = update.Name;
            vm.TypeName = update.TypeName;
            vm.BuildDate = update.BuildDate;
            vm.Crew = update.Crew;
            vm.EnginePower = update.EnginePower;
            vm.Passengers = update.Passengers;
            vm.InnerVolume = update.InnerVolume;
            vm.CreatedAt = update.CreatedAt;
            vm.ModifiedAt = update.ModifiedAt;

            return View("CreateUpdate", vm);
        }

        [HttpPost]
        public async Task<IActionResult>Update(KindergartenCreateUpdateVeiwModel vm)
        {
            var dto = new SpaceshipDto()
            {
                Id = vm.Id,
                Name = vm.Name,
                TypeName = vm.TypeName,
                BuildDate = vm.BuildDate,
                Crew = vm.Crew,
                EnginePower = vm.EnginePower,
                Passengers = vm.Passengers,
                InnerVolume = vm.InnerVolume,
                CreatedAt = vm.CreatedAt,
                ModifiedAt = vm.ModifiedAt,
            };

            var result = await _spaceshipsServices.Update(dto);

            if (result == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var spaceship = await _spaceshipsServices.DetailAsync(id);

            if (spaceship == null)
            {
                return NotFound();
            }

            var vm = new KindergartenDeleteViewModel();

            vm.Id = spaceship.Id;
            vm.Name = spaceship.Name;
            vm.TypeName = spaceship.TypeName;
            vm.BuildDate = spaceship.BuildDate;
            vm.Crew = spaceship.Crew;
            vm.EnginePower = spaceship.EnginePower;
            vm.Passengers = spaceship.Passengers;
            vm.InnerVolume = spaceship.InnerVolume;
            vm.CreatedAt = spaceship.CreatedAt;
            vm.ModifiedAt = spaceship.ModifiedAt;

            return View(vm);
        }
    }

}
