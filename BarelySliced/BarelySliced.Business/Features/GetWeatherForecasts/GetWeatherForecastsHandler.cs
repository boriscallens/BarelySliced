using BarelySliced.Persistence;

using MediatR;

namespace BarelySliced.Business.Features.GetWeatherForecasts
{
    public class GetWeatherForecastsHandler : IRequestHandler<GetWeatherForecastsRequest, GetWeatherForecastsResponse>
    {
        private readonly SliverDbContext db;

        public GetWeatherForecastsHandler(SliverDbContext db)
        {
            this.db = db;
        }
        public Task<GetWeatherForecastsResponse> Handle(GetWeatherForecastsRequest request, CancellationToken cancellationToken)
        {
            var forecasts = db.WeatherForecasts.Take(5).ToArray();
            return Task.FromResult(new GetWeatherForecastsResponse
            {
                Forecasts = forecasts
            });
        }
    }
}
