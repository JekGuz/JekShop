using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JekShop.Core.Domain;
using JekShop.Core.Dto;
using JekShop.Core.ServiceInterface;
using JekShop.Data;


namespace JekShop.ApplicationServices.Services
{
    public class SpaceshipsServices : ISpaceshipsServices
    {
        private readonly JekShopContext _context;

        // teha constructor
        public SpaceshipsServices
            (
                JekShopContext context
            )
        {
            _context = context;
        }
        public async Task<Spaceship> Create(SpaceshipDto dto)
        {
            Spaceship spaceship = new Spaceship();

            spaceship.Id = Guid.NewGuid();
            spaceship.Name = dto.Name;
            spaceship.TypeName = dto.TypeName;
            spaceship.BuildDate = dto.BuildDate;
            spaceship.Crew = dto.Crew;
            spaceship.EnginePower = dto.EnginePower;
            spaceship.Passengers = dto.Passengers;
            spaceship.InnerVolume = dto.InnerVolume;
            spaceship.CreatedAt = DateTime.Now;
            spaceship.ModifiedAt = DateTime.Now;

            await _context.Spaceships.AddAsync( spaceship );
            await _context.SaveChangesAsync();

            return spaceship;
        }
    }
}
