using BarelySliced.Domain;

using Microsoft.EntityFrameworkCore;

namespace BarelySliced.Persistence;

// Migration Commands
// dotnet ef migrations add "yourMigrationNameHere" --startup-project="BarelySliced.Api" --project="BarelySliced.Persistence"
// dotnet ef migrations remove --startup-project="BarelySliced.Api" --project="BarelySliced.Persistence"
public class SliverDbContext : DbContext
{
    public SliverDbContext(DbContextOptions<SliverDbContext> options) : base(options) { }

    public DbSet<WeatherForecast> WeatherForecasts { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WeatherForecast>()
            .HasKey(forecast => forecast.Date);

        SeedWeatherForecasts(modelBuilder);
    }

    private static void SeedWeatherForecasts(ModelBuilder modelBuilder)
    {
        var weatherForecasts = new WeatherForecast[]
        {
            new()
            {
                Date = new DateOnly(2024, 05, 16),
                TemperatureC = 25,
                Summary = "Warm"
            },
            new()
            {
                Date = new DateOnly(2024, 05, 17),
                TemperatureC = 30,
                Summary = "Hot"
            },
            new()
            {
                Date = new DateOnly(2024, 05, 18),
                TemperatureC = 15,
                Summary = "Cool"
            },
            new()
            {
                Date = new DateOnly(2024, 05, 19),
                TemperatureC = 10,
                Summary = "Chilly"
            },
            new()
            {
                Date = new DateOnly(2024, 05, 20),
                TemperatureC = 5,
                Summary = "Freezing"
            }
        };
        modelBuilder.Entity<WeatherForecast>().HasData(weatherForecasts);
    }
}