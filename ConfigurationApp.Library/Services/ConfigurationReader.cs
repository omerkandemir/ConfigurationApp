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
    private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1); 
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

    private async Task LoadConfigurations()
    {
        try
        {
            await _semaphore.WaitAsync(); // Thread-Safety için kilitle
            DbContextOptionsBuilder<ConfigurationDbContext> optionsBuilder = AddDatabase();
            using (var context = new ConfigurationDbContext(optionsBuilder.Options))
            {
                // ApplicationName'e göre filtrele ve konfigürasyonları çek
                var configurations = await context.Configurations
                    .Where(config => config.ApplicationName == _applicationName && config.IsActive)
                    .ToListAsync();

                // Yeni verileri cache'e ekle
                if (configurations != null && configurations.Count > 0)
                {
                    // Sadece başarılı veri çekme işlemlerinde cache'i güncelle
                    _configurationsCache.Clear(); // Eski cache'i temizle
                    foreach (var config in configurations)
                    {
                        _configurationsCache[config.Name] = config;
                    }
                    Console.WriteLine("Configurations loaded and cache updated.");
                }
            }
        }
        catch (Exception ex)
        {
            // Hata durumunda mevcut cache'i kullanmaya devam et
            Console.WriteLine("Error loading configurations: " + ex.Message);
            Console.WriteLine("Using cached configurations.");
        }
        finally
        {
            _semaphore.Release(); // Kilidi serbest bırak
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
