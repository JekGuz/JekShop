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
    }
}
