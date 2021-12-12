using Microsoft.EntityFrameworkCore;
using Middleware.Data.Model;

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

        public DbSet<Transaction> Transactions { get; set; }
    }
}
