namespace ConfigurationApp.Core.Entity.Abstract;

public interface IEntity
{
    public int Id { get; set; }
    public bool IsActive { get; set; }
}
