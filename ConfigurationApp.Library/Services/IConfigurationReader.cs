namespace ConfigurationApp.Library.Services;

public interface IConfigurationReader
{
    T GetValue<T>(string key);
}
