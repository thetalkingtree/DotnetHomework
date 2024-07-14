using DotnetHomework.Models;
using Microsoft.EntityFrameworkCore;

namespace DotnetHomework.Api.Repository;
public class FileDocumentRepository<T> : IDocumentRepository<T> where T : class
{
    public Task AddAsync(T entity)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<List<T>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<T> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(T entity)
    {
        throw new NotImplementedException();
    }
}