using Microsoft.EntityFrameworkCore;
using Data.Model;

namespace Data.DbContexts
{
    /// <summary>
    /// Context for work with db homework.
    /// </summary>
    public class HomeWorkDbContext : DbContext
    {
        public HomeWorkDbContext(DbContextOptions<HomeWorkDbContext> opt) : base(opt)
        {

        }

        public DbSet<CardModel> Cards { get; set; }

        public DbSet<TransactionModel> Transactions { get; set; }
    }
}
