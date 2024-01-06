using Microsoft.EntityFrameworkCore;
using WebApplication1.Pages.Auth;
// Alte directive `using` dacă este necesar

namespace WebApplication1.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<NewUserModel> users { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSet-uri pentru modelele dvs., de exemplu:
        // public DbSet<YourModel> YourModels { get; set; }
    }
}
