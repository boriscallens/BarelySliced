using MediatR;

namespace BarelySliced.Business.Features.GetWeatherForecasts;

public record GetWeatherForecastsRequest : IRequest<GetWeatherForecastsResponse>
{
}