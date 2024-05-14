using Microsoft.AspNetCore.Identity;

namespace ProductsAPI.Models
{
    public class CustomUser : IdentityUser
    {
        public ICollection<Product> Products { get; set; } = [];
    }
}
