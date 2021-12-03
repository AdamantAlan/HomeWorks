using Microsoft.EntityFrameworkCore;

namespace Middleware.Data.DbContexts
{
    /// <summary>
    /// Context for work with db homework.
    /// </summary>
    public class CardDbContext : DbContext
    {
        public CardDbContext(DbContextOptions<CardDbContext> opt) : base(opt)
        {

        }

        public DbSet<Card> Cards { get; set; }
    }
}
