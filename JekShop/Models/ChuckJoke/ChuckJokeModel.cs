namespace JekShop.Models.ChuckJoke
{
    public class ChuckJokeModel
    {
        public string? Id { get; set; }
        public string? Value { get; set; } // сама шутка
        public string? IconUrl { get; set; } // иконка
        public string? Url { get; set; } // ссылка на шутку
        public string? CreatedAt { get; set; }
        public string? UpdatedAt { get; set; }
    }
}