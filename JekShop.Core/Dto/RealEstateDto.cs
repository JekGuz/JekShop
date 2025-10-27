using Microsoft.AspNetCore.Http;

namespace JekShop.Core.Dto
{
    public class RealEstateDto
    {
        public Guid? Id { get; set; }
        public double? Area { get; set; }
        public string? Location { get; set; }
        public int? RoomNumber { get; set; }
        public string? BuildingType { get; set; }
        public List <IFormFile>? Files { get; set; }
        public IEnumerable<FileToDatabaseDto> Image { get; set; }
            = new List<FileToDatabaseDto>();
        public DateTime? CreateAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}
