using System.Linq;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    /// <summary>
    /// Repository cintract for database HomeWork.
    /// </summary>
    public interface IRepository
    {
        /// <summary>
        /// Write entity in database.
        /// </summary>
        Task<long> CreateAsync<T>(T entity) where T : IEntity;

        /// <summary>
        /// Check exist entity.
        /// </summary>
        /// <returns>true - entity exist;false - entity not exist</returns>
        Task<bool> EntityExist<T>(long id) where T : class, IEntity;

        /// <summary>
        /// Get all entity.
        /// </summary>
        IQueryable<T> GetAll<T>() where T : class, IEntity;

        /// <summary>
        /// Get entity by id.
        /// </summary>
        Task<T> GetAsync<T>(long id) where T : class, IEntity;

        /// <summary>
        /// Update entity.
        /// </summary>
        Task<long> UpdateAsync<T>(T entity) where T : IEntity;

        /// <summary>
        /// Delete entity.
        /// </summary>
        Task<bool> DeleteAsync<T>(T entity) where T : class, IEntity;

        /// <summary>
        /// Save entity.
        /// </summary>
        Task<bool> SaveChangeAsync();
    }
}
