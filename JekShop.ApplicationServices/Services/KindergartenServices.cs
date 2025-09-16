using System;
using System.Collections.Generic;
using System.Linq;
using JekShop.Core.Domain;
using JekShop.Core.Dto;
using JekShop.Core.ServiceInterface;
using JekShop.Data;
using Microsoft.EntityFrameworkCore;



namespace JekShop.ApplicationServices.Services
{
    public class KindergartenServices : IKindergartenServices
    {
        private readonly JekShopContext _context;

        // teha constructor
        public KindergartenServices
            (
                JekShopContext context
            )
        {
            _context = context;
        }
        public async Task<Kindergarten> Create(KindergartenDto dto)
        {
            Kindergarten kindergarten = new Kindergarten();

            Kindergarten.Id = Guid.NewGuid();
            Kindergarten.GroupName = dto.GroupName;
            Kindergarten.ChildrenCount = dto.ChildrenCount;
            Kindergarten.BuildDate = dto.BuildDate;
            Kindergarten.KindergartenName = dto.KindergartenName;
            Kindergarten.TeacherName = dto.TeacherName;
            Kindergarten.CreatedAt = DateTime.Now;
            Kindergarten.UpdateAt = DateTime.Now;


            await _context.Kindergarten.AddAsync( kindergarten );
            await _context.SaveChangesAsync();

            return kindergarten;
        }

        public async Task<Kindergarten> DetailAsync(Guid id)
        {
            var result = await _context.Kindergarten
                .FirstOrDefaultAsync( x => x.Id == id );

            return result;
        }
        public async Task <Kindergarten>Delete(Guid id)
        {
            var remove = await _context.Kindergarten
                .FirstOrDefaultAsync(x => x.Id == id);

            _context.Kindergarten.Remove(remove);
            await _context.SaveChangesAsync();

            return remove;
        }
        public async Task<Kindergarten> Update(KindergartenDto dto)
        {
            Kindergarten domain = new();

            domain.Id = dto.Id;
            domain.Id = Guid.NewGuid();
            domain.GroupName = dto.GroupName;
            domain.ChildrenCount = dto.ChildrenCount;
            domain.BuildDate = dto.BuildDate;
            domain.KindergartenName = dto.KindergartenName;
            domain.TeacherName = dto.TeacherName;
            domain.CreatedAt = DateTime.Now;
            domain.UpdateAt = DateTime.Now;

            _context.Kindergartens.Update(domain);
            await _context.SaveChangesAsync();

            return domain;
        }
    }
}
