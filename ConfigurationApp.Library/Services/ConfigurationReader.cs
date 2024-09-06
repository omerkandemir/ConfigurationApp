using ConfigurationApp.DataAccess.Concrete.EntityFramework;
using ConfigurationApp.Entities;
using Microsoft.EntityFrameworkCore;
using System.Timers;

namespace ConfigurationApp.Library.Services;

public class ConfigurationReader : IConfigurationReader
{
    private readonly System.Timers.Timer _timer;
    private Dictionary<string, Configuration> _configurationsCache;
    private readonly string _applicationName;
    private readonly string _connectionString;
    public ConfigurationReader(string applicationName, string connectionString, double refreshTimerIntervalInMs)
    {
        _configurationsCache = new Dictionary<string, Configuration>();
        _applicationName = applicationName;
        _connectionString = connectionString;
        // Timer'ı başlat
        _timer = new System.Timers.Timer(refreshTimerIntervalInMs);
        _timer.Elapsed += RefreshConfigurations;

        _timer.AutoReset = true; // Zamanlayıcı her seferinde tekrar edecek
        _timer.Start();

        // İlk başlatmada konfigürasyonların yüklenmesi
        LoadConfigurations();
    }

    private void RefreshConfigurations(object? sender, ElapsedEventArgs e)
    {
        // Yeni konfigürasyonları güncelle
        LoadConfigurations();
    }

    private async void LoadConfigurations()
    {
        try
        {
            DbContextOptionsBuilder<ConfigurationDbContext> optionsBuilder = AddDatabase();
            using (var context = new ConfigurationDbContext(optionsBuilder.Options))
            {
                // ApplicationName'e göre filtrele ve konfigürasyonları çek
                var configurations = await context.Configurations
                    .Where(config => config.ApplicationName == _applicationName && config.IsActive)
                    .ToListAsync();
                Console.WriteLine($"Using ApplicationName: {_applicationName}");

                foreach (var config in configurations)
                {
                    if (!_configurationsCache.ContainsKey(config.Name))
                    {
                        _configurationsCache.Add(config.Name, config);
                    }
                    else
                    {
                        _configurationsCache[config.Name] = config; // Değişiklik varsa güncelle
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Storage'a ulaşılamazsa bir hata loglanabilir
            Console.WriteLine("Error loading configurations: " + ex.Message);
        }
    }

    private DbContextOptionsBuilder<ConfigurationDbContext> AddDatabase()
    {
        var optionsBuilder = new DbContextOptionsBuilder<ConfigurationDbContext>();
        optionsBuilder.UseSqlServer(_connectionString);
        return optionsBuilder;
    }

    public T GetValue<T>(string key)
    {
        if (_configurationsCache.ContainsKey(key))
        {
            var value = _configurationsCache[key].Value;
            return (T)Convert.ChangeType(value, typeof(T));
        }
        else
        {
            throw new KeyNotFoundException($"Configuration key '{key}' not found.");
        }
    }
}
