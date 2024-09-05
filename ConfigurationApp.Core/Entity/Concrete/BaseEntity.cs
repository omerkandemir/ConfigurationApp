using ConfigurationApp.Core.Entity.Abstract;

namespace ConfigurationApp.Core.Entity.Concrete;

public class BaseEntity : IEntity
{
    public int Id { get; set; }
    public bool IsActive { get; set; }
}
