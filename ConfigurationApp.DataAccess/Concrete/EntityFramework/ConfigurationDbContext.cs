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
    //public ConfigurationDbContext()
    //{

    //}
    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //{
    //    if (!optionsBuilder.IsConfigured)
    //    {
    //        optionsBuilder.UseSqlServer(@"Data Source=(localdb)\Omer; Initial Catalog = ConfigurationDB;");
    //    }
    //}
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new ConfigurationEntityConfiguration());
    }
}
