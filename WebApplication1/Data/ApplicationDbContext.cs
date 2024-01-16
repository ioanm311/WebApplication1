using Microsoft.EntityFrameworkCore;
using WebApplication1.Pages.Auth;
using static ShopModel;
// Alte directive `using` dacă este necesar

namespace WebApplication1.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<NewUserModel> users { get; set; }
        public DbSet<Product> products { get; set; }
        public DbSet<ShoppingCartItem> ShoppingCart { get; set; }
        public DbSet<SpinResultType> SpinResults { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSet-uri pentru modelele dvs., de exemplu:
        // public DbSet<YourModel> YourModels { get; set; }
    }
}
