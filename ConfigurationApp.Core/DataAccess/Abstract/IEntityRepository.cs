using ConfigurationApp.Core.Entity.Abstract;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace ConfigurationApp.Core.DataAccess.Abstract;

public interface IEntityRepository<T> where T : class, IEntity, new()
{
    Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter = null,
        Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);
    Task<T> GetAsync(Expression<Func<T, bool>> filter = null,
        Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
}
