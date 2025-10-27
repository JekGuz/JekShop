namespace JekShop.Models.RealEstate
{
    public class RealEstateImageVeiwModel
    {
        public Guid Id { get; set; }
        public string? ImageTitle { get; set; }
        public byte[]? ImageData { get; set; }
        public string? Image { get; set; }
        public Guid? RealEstateId { get; set; }
    }
}
