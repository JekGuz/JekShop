using System;
using System.Collections.Generic;
using System.Linq;
using JekShop.Core.Domain;
using JekShop.Core.Dto;
using JekShop.Core.ServiceInterface;
using JekShop.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;



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

            kindergarten.Id = Guid.NewGuid();
            kindergarten.GroupName = dto.GroupName;
            kindergarten.ChildrenCount = (int)dto.ChildrenCount;
            kindergarten.KindergartenName = dto.KindergartenName;
            kindergarten.TeacherName = dto.TeacherName;
            kindergarten.CreateAt = DateTime.Now;
            kindergarten.UpdateAt = DateTime.Now;


            await _context.Kindergartens.AddAsync( kindergarten );
            await _context.SaveChangesAsync();

            return kindergarten;
        }

        public async Task<Kindergarten> DetailAsync(Guid id)
        {
            var result = await _context.Kindergartens
                .FirstOrDefaultAsync( x => x.Id == id );

            return result;
        }
        public async Task <Kindergarten>Delete(Guid id)
        {
            var remove = await _context.Kindergartens
                .FirstOrDefaultAsync(x => x.Id == id);

            _context.Kindergartens.Remove(remove);
            await _context.SaveChangesAsync();

            return remove;
        }
        public async Task<Kindergarten> Update(KindergartenDto dto)
        {
            Kindergarten domain = new();

            domain.Id = (Guid)dto.Id;
            domain.GroupName = dto.GroupName;
            domain.ChildrenCount = (int)dto.ChildrenCount;
            domain.KindergartenName = dto.KindergartenName;
            domain.TeacherName = dto.TeacherName;
            domain.CreateAt = dto.CreateAt;
            domain.UpdateAt = DateTime.Now;

            _context.Kindergartens.Update(domain);
            await _context.SaveChangesAsync();

            return domain;
        }
    }
}
