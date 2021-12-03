using Microsoft.EntityFrameworkCore;

namespace Middleware.Data.DbContexts
{
    /// <summary>
    /// Context for work with db homework.
    /// </summary>
    public class HomeWorkDbContext : DbContext
    {
        public HomeWorkDbContext(DbContextOptions<HomeWorkDbContext> opt) : base(opt)
        {

        }

        public DbSet<Card> Cards { get; set; }
    }
}
