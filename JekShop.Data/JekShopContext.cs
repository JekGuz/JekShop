using JekShop.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace JekShop.Data
{
    public class JekShopContext : IdentityDbContext<ApplicationUser>
    {
        public JekShopContext(DbContextOptions<JekShopContext> options) : base(options) 
        { 
        }
        public DbSet<Spaceship> Spaceships { get; set; }

        public DbSet<FileToApi> FileToApis { get; set; }

        public DbSet<RealEstate> RealEstates { get; set; }

        public DbSet<FileToDatabase> FileToDatabases { get; set; }

        public DbSet<IdentityRole> IdentityRoles { get; set; }


    }
}
