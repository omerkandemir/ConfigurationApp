using ConfigurationApp.Core.DataAccess.Abstract;
using ConfigurationApp.Core.Entity.Abstract;

namespace ConfigurationApp.Business.Concrete.BaseManager;

public abstract class BaseManager<T, Tdal>
    where T : class, IEntity, new()
    where Tdal : IEntityRepository<T>
{
    protected readonly Tdal _tdal;

    protected BaseManager(Tdal tdal)
    {
        _tdal = tdal;
    }

    public virtual async Task AddAsync(T value)
    {
        try
        {
           await _tdal.AddAsync(value);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public virtual async Task UpdateAsync(T value)
    {
        try
        {
           await _tdal.UpdateAsync(value);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public virtual async Task DeleteAsync(T value)
    {
        try
        {
            await _tdal.DeleteAsync(value);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public virtual async Task<T> GetAsync(int Id)
    {
        try
        {
            return await _tdal.GetAsync(x=>x.Id == Id && x.IsActive == true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public virtual async Task<List<T>> GetAllAsync()
    {
        try
        {
            return await _tdal.GetAllAsync(x=>x.IsActive == true);
        }
        catch (Exception)
        {

            throw;
        }
    }
}
