namespace JekShop.Models.RealEstate
{
    public class RealEstateDetailsViewModel
    {
        public Guid? Id { get; set; }
        public double? Area { get; set; }
        public string? Location { get; set; }
        public int? RoomNumber { get; set; }
        public string? BuildingType { get; set; }

        public List<RealEstate.RealEstateImageVeiwModel> Images { get; set; }
            = new List<RealEstate.RealEstateImageVeiwModel>();
        public DateTime? CreateAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}
