using ConfigurationApp.Core.Entity.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConfigurationApp.Core.DataAccess.Concrete.EntityFramework.Configuration;

public class BaseEntityConfiguration<T> : IEntityTypeConfiguration<T> where T : class, IEntity, new()
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder.Property<int>("Id").HasColumnName("Id");
        builder.HasKey("Id");
        builder.Property(x => x.IsActive).HasColumnName("IsActive");
    }
}
