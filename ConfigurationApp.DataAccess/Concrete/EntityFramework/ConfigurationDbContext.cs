using ConfigurationApp.DataAccess.Concrete.EntityFramework.EfConfiguration;
using ConfigurationApp.Entities;
using Microsoft.EntityFrameworkCore;

namespace ConfigurationApp.DataAccess.Concrete.EntityFramework;

public class ConfigurationDbContext : DbContext
{
    public DbSet<Configuration> Configurations { get; set; }
    public ConfigurationDbContext(DbContextOptions<ConfigurationDbContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new ConfigurationEntityConfiguration());
    }
}
