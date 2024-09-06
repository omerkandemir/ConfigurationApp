using ConfigurationApp.Core.Business.Abstract;
using ConfigurationApp.Entities;

namespace ConfigurationApp.Business.Abstract;

public interface IConfigurationService : IEntityServiceRepository<Configuration>
{
    Task<Configuration> GetByNameAsync(string name);
    Task<Configuration> GetByApplicationNameAsync(string applicationName);
}
