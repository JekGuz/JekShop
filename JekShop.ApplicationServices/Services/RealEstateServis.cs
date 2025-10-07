using System.Xml;
using JekShop.Core.Domain;
using JekShop.Core.Dto;
using JekShop.Core.ServiceInterface;
using JekShop.Data;
using Microsoft.EntityFrameworkCore;

namespace JekShop.ApplicationServices.Services
{
    public class RealEstateServis : IRealEstateServices
    {
        private readonly JekShopContext _context;
        private readonly IFileServices _fileServices;

        public RealEstateServis
            (
            JekShopContext context,
            IFileServices fileServices
            )
        {
            _context = context;
            _fileServices = fileServices;
        }
        public async Task <RealEstate> Create(RealEstateDto dto)
        {
            RealEstate realestate = new RealEstate();

            realestate.Id = Guid.NewGuid();
            realestate.Area = dto.Area;
            realestate.Location = dto.Location;
            realestate.RoomNumber = dto.RoomNumber;
            realestate.BuildingType = dto.BuildingType;
            realestate.CreateAt = DateTime.Now;
            realestate.ModifiedAt = DateTime.Now;

            // peaks kontrollima, kas failid on olemas või mitte
            if (dto.Files != null)
            {
                _fileServices.UploadFilesToDatabase(dto, realestate);
            }
            

            await _context.RealEstates.AddAsync(realestate);
            await _context.SaveChangesAsync();

            return realestate;
        }

        public async Task<RealEstate> DetailAsync(Guid id)
        {
            var result = await _context.RealEstates
                .FirstOrDefaultAsync(x => x.Id == id);

            return result;
        }
        public async Task<RealEstate> Delete(Guid id)
        {
            var remove = await _context.RealEstates
                .FirstOrDefaultAsync(x => x.Id == id);

            _context.RealEstates.Remove(remove);
            await _context.SaveChangesAsync();

            return remove;
        }
        public async Task<RealEstate> Update(RealEstateDto dto)
        {
            RealEstate domain = new();

            domain.Id = dto.Id;
            domain.Area = dto.Area;
            domain.Location = dto.Location;
            domain.RoomNumber = dto.RoomNumber;
            domain.BuildingType = dto.BuildingType;
            domain.CreateAt = dto.CreateAt;
            domain.ModifiedAt = DateTime.Now;

            _context.RealEstates.Update(domain);
            await _context.SaveChangesAsync();

            return domain;
        }
    }
}
