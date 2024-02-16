using MediatR;

namespace BarelySliced.Business.Features.MyFeature;

public record MyFeatureRequest : IRequest<MyFeatureResponse>
{
}