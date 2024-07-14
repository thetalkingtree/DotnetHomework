using DotnetHomework.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DotnetHomework.Api.Repository
{
    /// <summary>
    /// Represents a SQL-based repository for managing documents.
    /// </summary>
    /// <typeparam name="T">The type of the entity managed by the repository.</typeparam>
    public class SqlDocumentRepository<T> : IDocumentRepository<Document> where T : class
    {
        private readonly DataContext _context;
        private readonly DbSet<Document> _dbSet;

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlDocumentRepository{T}"/> class.
        /// </summary>
        /// <param name="context">The data context to be used by the repository.</param>
        public SqlDocumentRepository(DataContext context)
        {
            _context = context;
            _dbSet = _context.Set<Document>();
        }

        /// <summary>
        /// Asynchronously gets a document by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the document.</param>
        /// <returns>A task representing the asynchronous operation, with the document as the result.</returns>
        public async Task<Document> GetByIdAsync(int id)
        {
            var retVal = await _context.Documents
                .Where(doc => doc.Id == id)
                .Include(i => i.Data)
                .Include(tags => tags.DocumentTags)
                .ThenInclude(ti => ti.Tag)
                .FirstOrDefaultAsync();
#pragma warning disable CS8603 // Possible null reference return.
            return retVal;
#pragma warning restore CS8603 // Possible null reference return.
        }

        /// <summary>
        /// Asynchronously gets all documents.
        /// </summary>
        /// <returns>A task representing the asynchronous operation, with a list of documents as the result.</returns>
        public async Task<List<Document>> GetAllAsync()
        {
            var retVal = await _dbSet
                .Include(i => i.Data)
                .Include(tags => tags.DocumentTags)
                .ThenInclude(ti => ti.Tag)
                .ToListAsync();
            return retVal;
        }

        /// <summary>
        /// Asynchronously adds a new document.
        /// </summary>
        /// <param name="entity">The document to add.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task AddAsync(Document entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Asynchronously updates an existing document.
        /// </summary>
        /// <param name="entity">The document to update.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task UpdateAsync(Document entity)
        {
            if (await _dbSet.FindAsync(entity.Id) == null)
            {
                throw new KeyNotFoundException(string.Format("Entity not found {0} {1}", entity.GetType().FullName, entity.Id));
            }

            else
            {
                _dbSet.Update(entity);
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Asynchronously deletes a document by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the document to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }

            else
            {
                throw new KeyNotFoundException(string.Format("Entity key not found {0} ", id));
            }
        }
    }
}
