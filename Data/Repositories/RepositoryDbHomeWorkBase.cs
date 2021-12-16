using Microsoft.EntityFrameworkCore;
using Data.DbContexts;
using Data.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class RepositoryDbHomeWorkBase : IRepository
    {
        private readonly HomeWorkDbContext _db;

        public RepositoryDbHomeWorkBase(HomeWorkDbContext db)
        {
            _db = db;
        }

        public async Task<long> CreateAsync<T>(T entity) where T : IEntity
        {
            await _db.AddAsync(entity);
            await SaveChangeAsync();

            return entity.Id;
        }

        public async Task<bool> DeleteAsync<T>(T entity) where T : class, IEntity
        {
            _db.Remove(entity);
            return await SaveChangeAsync();
        }

        public async Task<long> UpdateAsync<T>(T entity) where T : IEntity
        {
            _db.Update(entity);
            await SaveChangeAsync();

            return entity.Id;
        }

        public async Task<bool> EntityExist<T>(long id) where T : class, IEntity => await _db.FindAsync<T>(id) != null;

        public IQueryable<T> GetAll<T>() where T : class, IEntity => _db.Set<T>();

        public async Task<T> GetAsync<T>(long id) where T : class, IEntity => await GetAll<T>().FirstOrDefaultAsync(e => e.Id == id);

        public async Task<bool> SaveChangeAsync() => await _db.SaveChangesAsync() > 0;
    }
}