using System;
using JekShop.Core.Domain;
using JekShop.Core.Dto;

namespace JekShop.Core.ServiceInterface
{
    public interface ISpaceshipsServices
    {
        Task<Spaceship> Create(SpaceshipDto dto);
        Task<Spaceship> DetailAsync(Guid id);
        Task<Spaceship> Delete(Guid id);
    }
}
