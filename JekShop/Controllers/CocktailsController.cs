using JekShop.Core.ServiceInterface;
using JekShop.Models.Cocktail;
using Microsoft.AspNetCore.Mvc;

namespace JekShop.Controllers
{
    public class CocktailsController : Controller
    {
        private readonly ICocktailServices _service;
        
        public CocktailsController(ICocktailServices service) => _service = service;

        [HttpGet]
        public IActionResult Index() => View();

        [HttpPost]
        public async Task<IActionResult> Search(string name)
        {
            // Проверка на пустой или пробельный ввод
            if (string.IsNullOrWhiteSpace(name))
                return View("Result", new List<CocktailViewModel>());

            // Вызов сервиса для поиска коктейлей по имени
            var data = await _service.SearchAsync(name);
            // Сохранение поискового запроса для отображения в представлении результатов
            ViewBag.SearchTerm = name; // сохраняем, чтобы показать в Result

            // Проверка результатов
            // Если нет данных, возвращаем пустой список
            // или если массив коктейлей пуст
            // Возвращаем представление "Result" с пустым списком
            if (data?.drinks == null || data.drinks.Length == 0)
                return View("Result", new List<CocktailViewModel>());

            // Преобразование данных в модель представления
            // Проекция каждого коктейля в CocktailViewModel
            var list = data.drinks.Select(d => new CocktailViewModel
            {
                Id = d.idDrink,
                Name = d.strDrink,
                Category = d.strCategory,
                Alcoholic = d.strAlcoholic,
                Glass = d.strGlass,
                Instructions = d.strInstructions,
                ThumbUrl = d.strDrinkThumb,
                // Сбор ингредиентов и мер в список
                Ingredients = new[]
                {
            (d.strIngredient1, d.strMeasure1),
            (d.strIngredient2, d.strMeasure2),
            (d.strIngredient3, d.strMeasure3),
            (d.strIngredient4, d.strMeasure4),
            (d.strIngredient5, d.strMeasure5),
            (d.strIngredient6, d.strMeasure6),
            (d.strIngredient7, d.strMeasure7),
        }
                // Фильтрация и форматирование ингредиентов
                // Удаление пустых ингредиентов и объединение с мерами
                // Преобразование в строку и удаление лишних пробелов
                .Where(x => !string.IsNullOrWhiteSpace(x.Item1))
                .Select(x => $"{x.Item1} {x.Item2}".Trim())
                .ToList()
            }).ToList();

            return View("Result", list);
        }

    }
}
