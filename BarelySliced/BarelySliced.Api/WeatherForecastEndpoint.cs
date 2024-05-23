using BarelySliced.Business.Features.GetWeatherForecasts;
using BarelySliced.Domain.Authorization;

using MediatR;

namespace BarelySliced.Api
{
    public static class WeatherForecastEndpoint
    {
        public static void MapWeatherForecastEndpoint(WebApplication app)
        {
            app.MapGet("/weatherforecast", async (IMediator mediator) =>
            {
                var request = new GetWeatherForecastsRequest();
                var response = await mediator.Send(request);
                return response;
            })
            .RequireAuthorization(Policies.ManageWeatherForecast)
            .WithName("GetWeatherForecast")
            .WithOpenApi();
        }
    }
}