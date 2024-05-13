using BarelySliced.Business.Features.GetWeatherForecasts;
using BarelySliced.Domain;
using BarelySliced.Persistence;

using Microsoft.Extensions.DependencyInjection;

namespace BarelySliced.Business.Tests.Features.GetWeatherForecasts;

public class GetWeatherForecastsShould : IClassFixture<UnitTestFixture>
{
    private readonly SliverDbContext db;

    public GetWeatherForecastsShould(UnitTestFixture fixture)
    {
        db = fixture.GetRequiredService<SliverDbContext>();
    }

    [Fact]
    public async Task GetNextFiveForecasts()
    {
        var forecasts = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                {
                    Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = WeatherSummaries.Summaries[Random.Shared.Next(WeatherSummaries.Summaries.Length)]
                }).ToArray();
        db.AddRange(forecasts);
        db.SaveChanges();

        var request = new GetWeatherForecastsRequest();
        var handler = new GetWeatherForecastsHandler(db);
        var response = await handler.Handle(request, CancellationToken.None);

        Assert.Equal(5, response.Forecasts.Length);
    }
}