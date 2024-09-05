using ConfigurationApp.Core.DataAccess.Abstract;
using ConfigurationApp.Core.Entity.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace ConfigurationApp.Core.DataAccess.Concrete;

public class EfEntityRepositoryBase<TEntity, TContext> : IEntityRepository<TEntity>
    where TEntity : class, IEntity, new()
    where TContext : DbContext
{
    protected readonly TContext _context;
    // DbContext'i DI (Dependency Injection) ile alacak constructor
    public EfEntityRepositoryBase(TContext context)
    {
        _context = context;
    }
    public async Task AddAsync(TEntity entity)
    {
        await CrudOperation(entity, EntityState.Added);
    }

    public async Task UpdateAsync(TEntity entity)
    {
       await CrudOperation(entity, EntityState.Modified);
    }
    public async Task DeleteAsync(TEntity entity)
    {
      await CrudOperation(entity, EntityState.Deleted);
    }
    public async Task<TEntity> GetAsync(
        Expression<Func<TEntity, bool>> filter,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null)
    {
        IQueryable<TEntity> query = Query(include);

        return await query.FirstOrDefaultAsync(filter);
    }

    public async  Task<List<TEntity>> GetAllAsync(
        Expression<Func<TEntity, bool>> filter = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null)
    {
        IQueryable<TEntity> query = Query(include);

        return filter == null ? await query.ToListAsync() : await query.Where(filter).ToListAsync();
    }

    private IQueryable<TEntity> Query(Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include)
    {
        IQueryable<TEntity> query = _context.Set<TEntity>();

        if (include != null)
        {
            query = include(query);
        }

        return query;
    }

    private async Task CrudOperation(TEntity entity, EntityState entityState)
    {
        try
        {
            var entry = _context.Entry(entity);
            switch (entityState)
            {
                case EntityState.Added:
                    entry.State = EntityState.Added;
                    break;
                case EntityState.Deleted:
                    entry.State = EntityState.Deleted;
                    break;
                case EntityState.Modified:
                    _context.Attach(entity); // Entity'yi attach et
                    entry.State = EntityState.Modified;
                    break;
                default:
                    break;
            }
           await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {

            throw new Exception(ex.Message);
        }
    }
}
