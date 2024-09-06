namespace ConfigurationApp.Library.Services;

public class ConfigurationReaderFactory : IConfigurationReaderFactory
{
    public IConfigurationReader Create(string applicationName, string connectionString, double refreshTimerIntervalInMs)
    {
        return new ConfigurationReader(applicationName, connectionString, refreshTimerIntervalInMs);
    }
}
