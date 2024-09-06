namespace ConfigurationApp.Library.Services;

public interface IConfigurationReaderFactory
{
    ConfigurationReader Create(string applicationName, string connectionString, double refreshTimerIntervalInMs);
}
