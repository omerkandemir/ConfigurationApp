using ConfigurationApp.Business.Abstract;
using ConfigurationApp.Business.Concrete.BaseManager;
using ConfigurationApp.DataAccess.Abstract;
using ConfigurationApp.Entities;

namespace ConfigurationApp.Business.Concrete;

public class ConfigurationManager : BaseManager<Configuration, IConfigurationDal>, IConfigurationService
{
    public ConfigurationManager(IConfigurationDal tdal) : base(tdal)
    {
    }
}
