using BarelySliced.Api;
using BarelySliced.Infrastructure;
using BarelySliced.Persistence;

using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.json");
builder.Configuration.AddEnvironmentVariables();

ConfigureServices(builder.Services, builder.Configuration, builder.Environment);

var weatherApp = builder.Build();

ConfigureMiddleware(weatherApp, builder.Configuration);
ConfigureEndpoints(weatherApp);

weatherApp.Run();
return;

void ConfigureServices(IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
{
    services.AddLogging();
    services.AddPersistence(configuration, environment.IsDevelopment());
    services.AddMediator();
    services.AddAuth(configuration);
    services.AddOpenApi(configuration);
}

void ConfigureMiddleware(WebApplication app, IConfiguration configuration)
{
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }

    app.UseOpenApi(configuration);
    app.UseHttpsRedirection();
    app.Services.CreateScope().ServiceProvider.GetRequiredService<SliverDbContext>().Database.Migrate();
}

void ConfigureEndpoints(WebApplication app)
{
    WeatherForecastEndpoint.MapWeatherForecastEndpoint(app);
}
