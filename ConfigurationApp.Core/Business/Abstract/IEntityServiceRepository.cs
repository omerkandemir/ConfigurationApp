using ConfigurationApp.Core.Entity.Abstract;

namespace ConfigurationApp.Core.Business.Abstract;

public interface IEntityServiceRepository<T>
    where T : class, IEntity, new()
{
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
    Task<List<T>> GetAllAsync();
    Task<T> GetAsync(int id);
}
