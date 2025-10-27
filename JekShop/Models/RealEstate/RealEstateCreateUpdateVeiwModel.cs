using System.Data;

namespace JekShop.Models.RealEstate
{
    public class RealEstateCreateUpdateVeiwModel
    {
        public Guid? Id { get; set; }
        public double? Area { get; set; }
        public string? Location { get; set; }
        public int? RoomNumber { get; set; }
        public string? BuildingType { get; set; }
        public List<IFormFile>? Files { get; set; }
        public List<RealEstateImageVeiwModel> Images { get; set; }
            = new List<RealEstateImageVeiwModel>();
        public DateTime? CreateAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}
