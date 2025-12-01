namespace JekShop.Models.Email
{
    public class EmailViewModel
    {
        public string? To { get; set; }
        public string? Subject { get; set; }
        public string? Body { get; set; }
        public IFormCollection? Attrachment { get; set; }
    }
}
