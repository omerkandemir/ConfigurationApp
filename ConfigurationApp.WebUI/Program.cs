using ConfigurationApp.Business.Abstract;
using ConfigurationApp.DataAccess.Abstract;
using ConfigurationApp.DataAccess.Concrete.EntityFramework;
using ConfigurationApp.Library.Services;
using ConfigurationApp.Library.Utilities;
using ConfigurationApp.WebUI.Controllers;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient<ConfigurationsController>();


// appsettings.json'dan connection string'i al
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// DbContext'i ekle
builder.Services.AddDbContext<ConfigurationDbContext>(options =>
{
    var configuration = builder.Configuration.GetSection("ConfigurationSettings").Get<ConfigurationSettings>();
    options.UseSqlServer(configuration.ConnectionString);
});

// ConfigurationSettings'i ayarla
builder.Services.Configure<ConfigurationSettings>(builder.Configuration.GetSection("ConfigurationSettings"));

// Business katmaný servislerini ekle
builder.Services.AddScoped<IConfigurationService, ConfigurationApp.Business.Concrete.ConfigurationManager>();
builder.Services.AddScoped<IConfigurationDal, EfConfigurationDal>();
builder.Services.AddScoped<IConfigurationReaderFactory, ConfigurationReaderFactory>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Configurations}/{action=Index}/{id?}");

app.Run();
