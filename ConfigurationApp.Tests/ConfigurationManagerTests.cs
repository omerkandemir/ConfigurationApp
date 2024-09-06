using Microsoft.EntityFrameworkCore;
using ConfigurationApp.Business.Concrete;
using ConfigurationApp.DataAccess.Concrete.EntityFramework;
using ConfigurationApp.Entities;
using ConfigurationApp.Library.Services;
using ConfigurationApp.Business.Abstract;
using ConfigurationApp.ConfigManagement.API.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace ConfigurationApp.Tests
{
    public class ConfigurationManagerTests
    {
        private ConfigurationManager _configurationManager;
        private ConfigurationDbContext _context;

        // Her test öncesi veritabanını sıfırlamalıyız
        private void InitializeTestDatabase()
        {
            var options = new DbContextOptionsBuilder<ConfigurationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Her test için farklı bir veritabanı adı
                .Options;

            _context = new ConfigurationDbContext(options);

            var configurationDal = new EfConfigurationDal(_context);
            _configurationManager = new ConfigurationManager(configurationDal);

            _context.Configurations.Add(new Configuration
            {
                Id = 1,
                Name = "TestConfig",
                ApplicationName = "TestApp",
                Type = "string",
                Value = "SomeValue",
                IsActive = true
            });
            _context.SaveChanges();
        }

        [Fact]
        public async Task AddAsync_ShouldAddConfigurationToDatabase_WhenConfigurationIsValid()
        {
            InitializeTestDatabase();

            // Hazırlık
            var newConfig = new Configuration
            {
                Id = 2,
                Name = "NewConfig",
                ApplicationName = "NewApp",
                Type = "int",
                Value = "42",
                IsActive = true
            };

            // Eylem
            await _configurationManager.AddAsync(newConfig);

            // Doğrulama
            var configFromDb = await _context.Configurations.FindAsync(2);
            Assert.NotNull(configFromDb);
            Assert.Equal("NewConfig", configFromDb.Name);
            Assert.Equal("int", configFromDb.Type);
            Assert.Equal("42", configFromDb.Value);
        }

        [Fact]
        public async Task GetByApplicationNameAsync_ShouldReturnCorrectConfig_WhenConfigExists()
        {
            InitializeTestDatabase();

            // Eylem
            var config = await _configurationManager.GetByApplicationNameAsync("TestApp");

            // Doğrulama
            Assert.NotNull(config);
            Assert.Equal("TestApp", config.ApplicationName);
            Assert.Equal("TestConfig", config.Name);
            Assert.Equal("string", config.Type);
            Assert.Equal("SomeValue", config.Value);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateConfigurationInDatabase_WhenConfigurationIsValid()
        {
            InitializeTestDatabase();

            // Hazırlık
            var config = await _context.Configurations.FindAsync(1);
            config.Name = "UpdatedConfig";
            config.Type = "bool";
            config.Value = "true";

            // Eylem
            await _configurationManager.UpdateAsync(config);

            // Doğrulama
            var updatedConfig = await _context.Configurations.FindAsync(1);
            Assert.Equal("UpdatedConfig", updatedConfig.Name);
            Assert.Equal("bool", updatedConfig.Type);
            Assert.Equal("true", updatedConfig.Value);
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveConfigurationFromDatabase_WhenConfigurationIsValid()
        {
            InitializeTestDatabase();

            // Hazırlık
            var config = await _context.Configurations.FindAsync(1);

            // Eylem
            await _configurationManager.DeleteAsync(config);

            // Doğrulama
            var deletedConfig = await _context.Configurations.FindAsync(1);
            Assert.Null(deletedConfig);
        }

        [Fact]
        public async Task GetAsync_ShouldReturnCorrectConfig_WhenIdIsValid()
        {
            InitializeTestDatabase();

            // Eylem
            var config = await _configurationManager.GetAsync(1);

            // Doğrulama
            Assert.NotNull(config);
            Assert.Equal(1, config.Id);
            Assert.Equal("TestConfig", config.Name);
            Assert.Equal("string", config.Type);
            Assert.Equal("SomeValue", config.Value);
        }

        [Fact]
        public async Task GetAllConfigurations_ShouldReturnOnlyActiveConfigurations()
        {
            // Hazırlık
            InitializeTestDatabase(); // Her test için veritabanını sıfırlar

            // Eylem
            var activeConfigs = await _configurationManager.GetAllAsync();

            // Doğrulama
            Assert.All(activeConfigs, config => Assert.True(config.IsActive));
        }

        [Fact]
        public async Task ConfigureReader_ShouldReturnCorrectValue_WhenDataExists()
        {
            // Mock nesnelerini oluşturuyoruz
            var configurationReaderFactoryMock = new Mock<IConfigurationReaderFactory>();
            var configurationServiceMock = new Mock<IConfigurationService>();

            // Mock ConfigurationReader
            var configurationReaderMock = new Mock<IConfigurationReader>();
            configurationReaderMock
                .Setup(reader => reader.GetValue<string>(It.IsAny<string>()))
                .Returns("soty.io");

            // Mock ConfigurationReaderFactory ayarlanıyor
            configurationReaderFactoryMock
                .Setup(factory => factory.Create(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<double>()))
                .Returns(configurationReaderMock.Object);

            // Mock ConfigurationService ayarlanıyor
            var configuration = new Configuration
            {
                Id = 1,
                Name = "SiteName",
                Value = "soty.io",
                ApplicationName = "TestApp",
                Type = "string",
                IsActive = true
            };

            configurationServiceMock
                .Setup(service => service.GetByApplicationNameAsync(It.IsAny<string>()))
                .ReturnsAsync(configuration);

            // Controller'ı başlatıyoruz
            var controller = new ConfigurationController(configurationServiceMock.Object, configurationReaderFactoryMock.Object);

            // Eylem
            var result = await controller.ConfigureReader("TestApp", "someConnectionString", 5000);

            // Doğrulama
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("soty.io", okResult.Value);
        }

        [Fact]
        public void ConfigurationReader_ShouldThrowException_WhenKeyNotFound()
        {
            string applicationName = "TestApp";
            string connectionString = "FakeConnectionString"; 
            double refreshTimerIntervalInMs = 5000;

            var options = new DbContextOptionsBuilder<ConfigurationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase2")
                .Options;

            using (var context = new ConfigurationDbContext(options))
            {
            }

            var configurationReader = new ConfigurationReader(applicationName, connectionString, refreshTimerIntervalInMs);

            Assert.Throws<KeyNotFoundException>(() => configurationReader.GetValue<string>("NonExistentKey"));
        }
    }
}
