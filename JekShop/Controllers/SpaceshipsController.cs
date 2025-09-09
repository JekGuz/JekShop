using System.Security.AccessControl;
using JekShop.Core.Domain;
using JekShop.Core.Dto;
using JekShop.Core.ServiceInterface;
using JekShop.Data;
using JekShop.Models.Spaceships;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace JekShop.Controllers
{
    public class SpaceshipsController : Controller
    {
        private readonly JekShopContext _context;
        private readonly ISpaceshipsServices _spaceshipsServices;
        public SpaceshipsController
            (
                JekShopContext context,
                ISpaceshipsServices spaceshipsServices
            )
        {
            _context = context;
            _spaceshipsServices = spaceshipsServices;
        }

        public IActionResult Index()
        {
            var result = _context.Spaceships
                .Select(x => new SpaceshipsIndexViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    BuildDate = x.BuildDate,
                    TypeName = x.TypeName,
                    Crew = x.Crew,
                });


            return View(result);
        }
        [HttpGet]
        public IActionResult Create()
        {
            SpaceshipCreateViewModel result = new();
            return View(result);
        }
        [HttpPost]  
        public async Task<IActionResult> Create(SpaceshipCreateViewModel vm)
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
    }
}
