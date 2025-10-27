// JekShop/Models/Cocktail/CocktailViewModel.cs
namespace JekShop.Models.Cocktail
{
    public class CocktailViewModel
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Category { get; set; }
        public string? Alcoholic { get; set; }
        public string? Glass { get; set; }
        public string? Instructions { get; set; }

        // картинка
        public string? ThumbUrl { get; set; }

        // список ингредиентов (с дозировкой)
        public List<string> Ingredients { get; set; } = new();
    }
}
