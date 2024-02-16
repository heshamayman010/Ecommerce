using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Controllers.Models
{
    public class AppDbContext:IdentityDbContext<Appuser>
    {
        public AppDbContext()
        {

        }
        public AppDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Products> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Cart>carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }


    }
}
