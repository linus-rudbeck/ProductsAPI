using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProductsAPI.Models;

namespace ProductsAPI.Data
{
    public class ProductsAPIDbContext(DbContextOptions options) : IdentityDbContext<CustomUser>(options)
    {
        public DbSet<Product> Products { get; set; }
    }
}
