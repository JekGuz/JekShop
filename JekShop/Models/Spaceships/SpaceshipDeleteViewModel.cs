namespace JekShop.Models.Spaceships
{
    public class RealEstateDeleteViewModel
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
        public string? TypeName { get; set; }
        public DateTime? BuildDate { get; set; }
        public int? Crew { get; set; }
        public int? EnginePower { get; set; }
        public int? Passengers { get; set; }
        public int? InnerVolume { get; set; }

        public List<ImageVeiwModel> Images { get; set; }
            = new List<ImageVeiwModel>();

        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}
