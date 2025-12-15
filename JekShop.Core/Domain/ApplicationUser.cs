using Microsoft.AspNetCore.Identity;

namespace JekShop.Core.Domain
{
    public class ApplicationUser : IdentityUser
    {
        public string? Name { get; set; }
        public string? City { get; set; }
    }
}
