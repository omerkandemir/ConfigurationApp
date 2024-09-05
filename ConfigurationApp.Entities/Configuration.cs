using ConfigurationApp.Core.Entity.Concrete;

namespace ConfigurationApp.Entities;

public class Configuration : BaseEntity
{
    public string Name { get; set; }
    public string Type { get; set; }
    public string Value { get; set; }
    public string ApplicationName { get; set; }
}
