using ConfigurationApp.Core.DataAccess.Concrete.EntityFramework.Configuration;
using ConfigurationApp.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConfigurationApp.DataAccess.Concrete.EntityFramework.EfConfiguration;

public class ConfigurationEntityConfiguration : BaseEntityConfiguration<Configuration>
{
    public override void Configure(EntityTypeBuilder<Configuration> builder)
    {
        base.Configure(builder);
        builder.ToTable("Configurations");

        builder.Property(x => x.Name).HasColumnName("Name").HasMaxLength(100);
        builder.Property(x => x.Type).HasColumnName("Type").HasMaxLength(50);
        builder.Property(x => x.Value).HasColumnName("Value").HasMaxLength(255);
        builder.Property(x => x.ApplicationName).HasColumnName("ApplicationName").HasMaxLength(50);
    }
}
