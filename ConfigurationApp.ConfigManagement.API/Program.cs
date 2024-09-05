using ConfigurationApp.Business.Abstract;
using ConfigurationApp.DataAccess.Abstract;
using ConfigurationApp.DataAccess.Concrete.EntityFramework;
using Microsoft.EntityFrameworkCore;

internal class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // appsettings.json'dan connection string'i al
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

        // DbContext'i ekle
        builder.Services.AddDbContext<ConfigurationDbContext>(options =>
    options.UseSqlServer(connectionString));

        // Business katmaný servislerini ekle
        builder.Services.AddScoped<IConfigurationService, ConfigurationApp.Business.Concrete.ConfigurationManager>();
        builder.Services.AddScoped<IConfigurationDal, EfConfigurationDal>();
        
        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage(); //++
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}