using ConfigurationApp.Core.DataAccess.Abstract;
using ConfigurationApp.Entities;

namespace ConfigurationApp.DataAccess.Abstract;

public interface IConfigurationDal : IEntityRepository<Configuration>
{
}
