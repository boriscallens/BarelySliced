using BarelySliced.Api;
using BarelySliced.Infrastructure;
using BarelySliced.Persistence;

using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.json");
builder.Configuration.AddEnvironmentVariables();

ConfigureServices(builder.Services, builder.Configuration);
var weatherApp = builder.Build();

ConfigureMiddleware(weatherApp);
ConfigureEndpoints(weatherApp);

weatherApp.Run();
return;

void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    services.AddLogging();
    services.AddPersistence(configuration);
    services.AddMediator();

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();
}

void ConfigureMiddleware(WebApplication app)
{
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }

    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHttpsRedirection();
    app.Services.CreateScope().ServiceProvider.GetRequiredService<SliverDbContext>().Database.Migrate();
}

void ConfigureEndpoints(WebApplication app)
{
    WeatherForecastEndpoint.MapWeatherForecastEndpoint(app);
}
