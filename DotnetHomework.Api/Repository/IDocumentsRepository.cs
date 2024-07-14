using System.Collections.Generic;
using System.Threading.Tasks;

namespace DotnetHomework.Api.Repository
{
    /// <summary>
    /// Defines a generic repository interface for managing documents.
    /// </summary>
    /// <typeparam name="T">The type of the entity managed by the repository.</typeparam>
    public interface IDocumentRepository<T> where T : class
    {
        /// <summary>
        /// Asynchronously gets an entity by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the entity.</param>
        /// <returns>A task representing the asynchronous operation, with the entity as the result.</returns>
        Task<T> GetByIdAsync(int id);

        /// <summary>
        /// Asynchronously gets all entities.
        /// </summary>
        /// <returns>A task representing the asynchronous operation, with a list of entities as the result.</returns>
        Task<List<T>> GetAllAsync();

        /// <summary>
        /// Asynchronously adds a new entity.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task AddAsync(T entity);

        /// <summary>
        /// Asynchronously updates an existing entity.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task UpdateAsync(T entity);

        /// <summary>
        /// Asynchronously deletes an entity by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the entity to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task DeleteAsync(int id);
    }
}
