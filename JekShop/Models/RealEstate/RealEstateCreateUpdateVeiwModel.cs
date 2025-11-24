using System.ComponentModel.DataAnnotations;
using System.Data;

namespace JekShop.Models.RealEstate
{
    public class RealEstateCreateUpdateVeiwModel
    {
        public Guid? Id { get; set; }

        [Range(1, double.MaxValue, ErrorMessage = "Area must be greater than 0.")]
        public double? Area { get; set; }
        public string? Location { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Room number must be greater than 0.")]
        public int? RoomNumber { get; set; }
        public string? BuildingType { get; set; }
        public List<IFormFile>? Files { get; set; }
        public List<RealEstateImageVeiwModel> Images { get; set; }
            = new List<RealEstateImageVeiwModel>();
        public DateTime? CreateAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}
