using ConfigurationApp.Business.Abstract;
using ConfigurationApp.Business.Concrete.DependencyRevolvers.Ninject;
using ConfigurationApp.Entities;

internal class Program
{
    private static void Main(string[] args)
    {
        var configurationService = InstanceFactory.GetInstance<IConfigurationService>();

        //AddConfiguration(configurationService);

        // UpdateConfiguration(configurationService);

        ListAllConfigurations(configurationService);
    }

    private static void AddConfiguration(IConfigurationService configurationService)
    {
        var config = new Configuration
        {
            Name = "test",
            Type = "Int",
            Value = "50",
            ApplicationName = "SERVICE-B",
            IsActive = false
        };

        configurationService.Add(config);
        Console.WriteLine("Yeni Configuration eklendi: " + config.Name);
    }

    private static void UpdateConfiguration(IConfigurationService configurationService)
    {
        var config = configurationService.Get(4); // ID = 1 olan kaydı getir
        if (config != null)
        {
            config.Value = "Updated Site Title";
            configurationService.Update(config);
            Console.WriteLine("Configuration güncellendi: " + config.Name);
        }
        else
        {
            Console.WriteLine("Configuration bulunamadı.");
        }
    }

    private static void ListAllConfigurations(IConfigurationService configurationService)
    {
        var configurations = configurationService.GetAll();
        foreach (var config in configurations)
        {
            Console.WriteLine($"ID: {config.Id}, Name: {config.Name}, Value: {config.Value}, IsActive: {config.IsActive}");
        }
    }
}