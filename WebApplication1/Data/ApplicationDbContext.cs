using Microsoft.EntityFrameworkCore;
// Alte directive `using` dacă este necesar

namespace WebApplication1.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSet-uri pentru modelele dvs., de exemplu:
        // public DbSet<YourModel> YourModels { get; set; }
    }
}
