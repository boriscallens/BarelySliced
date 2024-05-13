using BarelySliced.Domain;

namespace BarelySliced.Business.Features.GetWeatherForecasts;

public record GetWeatherForecastsResponse
{
    public WeatherForecast[] Forecasts { get; set; }
}