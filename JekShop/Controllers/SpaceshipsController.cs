using System.Security.AccessControl;
using JekShop.Data;
using JekShop.Models.Spaceships;
using Microsoft.AspNetCore.Mvc;

namespace JekShop.Controllers
{
    public class SpaceshipsController : Controller
    {
        private readonly JekShopContext _context;
        public SpaceshipsController
            (
                JekShopContext context
            )
        {
            _context = context;
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
                });


            return View(result);
        }
    }
}
