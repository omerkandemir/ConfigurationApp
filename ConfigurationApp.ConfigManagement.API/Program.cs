using ConfigurationApp.Business.Abstract;
using ConfigurationApp.DataAccess.Abstract;
using ConfigurationApp.DataAccess.Concrete.EntityFramework;
using ConfigurationApp.Library.Services;
using ConfigurationApp.Library.Utilities;
using Microsoft.EntityFrameworkCore;

internal class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // appsettings.json'dan connection string'i al
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

        builder.Services.AddDbContext<ConfigurationDbContext>(options =>
        {
            var configuration = builder.Configuration.GetSection("ConfigurationSettings").Get<ConfigurationSettings>();
            options.UseSqlServer(configuration.ConnectionString);
        });

        builder.Services.Configure<ConfigurationSettings>(builder.Configuration.GetSection("ConfigurationSettings"));


        // Business katmaný servislerini ekle
        builder.Services.AddScoped<IConfigurationService, ConfigurationApp.Business.Concrete.ConfigurationManager>();
        builder.Services.AddScoped<IConfigurationDal, EfConfigurationDal>();
        builder.Services.AddScoped<IConfigurationReaderFactory, ConfigurationReaderFactory>();

        // Add services to the container.
        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}