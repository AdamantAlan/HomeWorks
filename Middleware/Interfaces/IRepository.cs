using Middleware.Data;
using Middleware.Dto;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Middleware.Interfaces
{
    /// <summary>
    /// Contract for work with user cards.
    /// </summary>
    public interface IRepository
    {
        /// <summary>
        /// Write card in database.
        /// </summary>
        /// <param name="card">user card.</param>
        Task<long> CreateAsync<T>(T entity) where T : IEntity;

        /// <summary>
        /// Check exist entity.
        /// </summary>
        /// <returns>true - entity exist;false - entity not exist</returns>
        Task<bool> EntityExist<T>(T entity) where T : IEntity;

        /// <summary>
        /// Get all cards of user.
        /// </summary>
        /// <param name="id">user id</param>
        IQueryable<T> GetAll<T>() where T : class, IEntity;

        /// <summary>
        /// Change cardholder for user cards.
        /// </summary>
        Task<long> UpdateAsync<T>(T entity) where T : IEntity;

        /// <summary>
        /// Delete user card.
        /// </summary>
        Task<long> DeleteAsync<T>(T entity) where T : IEntity;

        /// <summary>
        /// Save update entity.
        /// </summary>
        Task<bool> SaveChangeAsync();
    }
}
