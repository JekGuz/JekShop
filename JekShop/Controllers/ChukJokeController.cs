using JekShop.Core.Dto;
using JekShop.Core.ServiceInterface;
using JekShop.Models.ChuckJoke;
using Microsoft.AspNetCore.Mvc;

namespace JekShop.Controllers
{
    public class ChuckJokeController : Controller
    {
        private readonly IChuckJokeService _chuckJokeService;

        public ChuckJokeController(IChuckJokeService chuckJokeService)
        {
            _chuckJokeService = chuckJokeService;
        }

        // Открывается по адресу /ChuckJoke/Index
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // получаем случайную шутку из сервиса
            ChuckJokeDto dto = await _chuckJokeService.GetRandomAsync();

            // создаём модель для отображения
            var vm = new ChuckJokeModel
            {
                Id = dto.Id,
                Value = dto.Value,
                IconUrl = dto.IconUrl,
                Url = dto.Url,
                CreatedAt = dto.CreatedAt,
                UpdatedAt = dto.UpdatedAt
            };

            // возвращаем вью ChuckJoke.cshtml
            return View("ChuckJoke", vm);
        }

        // Кнопка для загрузки новой шутки
        [HttpPost]
        public IActionResult NewJoke()
        {
            return RedirectToAction("Index");
        }
    }
}
