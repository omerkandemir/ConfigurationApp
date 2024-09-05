using ConfigurationApp.Core.DataAccess.Concrete;
using ConfigurationApp.DataAccess.Abstract;
using ConfigurationApp.Entities;
namespace ConfigurationApp.DataAccess.Concrete.EntityFramework;

public class EfConfigurationDal : EfEntityRepositoryBase<Configuration, ConfigurationDbContext>, IConfigurationDal
{
    public EfConfigurationDal(ConfigurationDbContext context) : base(context)
    {
        
    }
}
