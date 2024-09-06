namespace ConfigurationApp.Library.Services;

public interface IConfigurationReaderFactory
{
    IConfigurationReader Create(string applicationName, string connectionString, double refreshTimerIntervalInMs);
}
