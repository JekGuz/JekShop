namespace JekShop.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }

    public class Error404ViewModel
    {
        public Guid WrongId { get; set; }
    }
}
