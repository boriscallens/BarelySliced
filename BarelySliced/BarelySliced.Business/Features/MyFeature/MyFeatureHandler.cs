using MediatR;

namespace BarelySliced.Business.Features.MyFeature
{
    public class MyFeatureHandler: IRequestHandler<MyFeatureRequest, MyFeatureResponse>
    {
        public Task<MyFeatureResponse> Handle(MyFeatureRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
