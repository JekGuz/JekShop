using JekShop.Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace JekShop.Data
{
    public class JekShopContext : DbContext
    {
        public JekShopContext(DbContextOptions<JekShopContext> options) : base(options) 
        { 
        }
        public DbSet<Spaceship> Spaceships { get; set; }

        public DbSet<FileToApi> FileToApis { get; set; }

        public DbSet<Kindergarten> Kindergartens { get; set; }

        public DbSet<FileToDatabase> FileToDatabases { get; set; }




    }
}
