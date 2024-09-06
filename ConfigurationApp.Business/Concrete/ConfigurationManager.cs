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

    public async Task<Configuration> GetByApplicationNameAsync(string applicationName)
    {
        return await _tdal.GetAsync(x => x.ApplicationName == applicationName && x.IsActive == true); // ApplicationName alanına göre sorgulama
    }

    public async Task<Configuration> GetByNameAsync(string name)
    {
        return await _tdal.GetAsync(x => x.Name == name && x.IsActive == true); // Name alanına göre sorgulama
    }
}
