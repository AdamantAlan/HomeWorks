using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Middleware.Data;
using Middleware.Data.DbContexts;
using Middleware.Dto;
using Middleware.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Middleware.Services
{
    public class RepositoryDbHomeWork : IRepository
    {
        private readonly HomeWorkDbContext _db;

        public RepositoryDbHomeWork(HomeWorkDbContext db)
        {
            _db = db;
        }

        public async Task<long> CreateAsync<T>(T entity) where T : IEntity
        {
            await _db.AddAsync(entity);
            await SaveChangeAsync();

            return entity.Id;
        }

        public async Task<long> DeleteAsync<T>(T entity) where T : IEntity
        {
            await Task.Run(() => { _db.Remove(entity); });
            await SaveChangeAsync();

            return entity.Id;
        }

        public async Task<bool> EntityExist<T>(T entity) where T : IEntity => await _db.FindAsync(typeof(IEntity), entity.Id) != null;


        public IQueryable<T> GetAll<T>() where T : class, IEntity => _db.Set<T>().AsNoTracking();

        public async Task<bool> SaveChangeAsync() => await _db.SaveChangesAsync() > 0;


        public async Task<long> UpdateAsync<T>(T entity) where T : IEntity
        {
            _db.Update(entity);
            await SaveChangeAsync();

            return entity.Id;
        }
    }
}
