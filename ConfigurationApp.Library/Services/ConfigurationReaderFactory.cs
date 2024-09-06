namespace ConfigurationApp.Library.Services;

public class ConfigurationReaderFactory : IConfigurationReaderFactory
{
    public ConfigurationReader Create(string applicationName, string connectionString, double refreshTimerIntervalInMs)
    {
        return new ConfigurationReader(applicationName, connectionString, refreshTimerIntervalInMs);
    }
}
