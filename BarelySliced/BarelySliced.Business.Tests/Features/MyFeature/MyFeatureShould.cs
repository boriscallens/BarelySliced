using BarelySliced.Business.Features.MyFeature;
using Microsoft.Extensions.DependencyInjection;

namespace BarelySliced.Business.Tests.Features.MyFeature;

public class MyFeatureShould: IClassFixture<UnitTestFixture>
{
    public MyFeatureShould(UnitTestFixture fixture)
    {
        // fixture.GetRequiredService<>();
    }

    [Fact]
    public async Task DoSomething()
    {
        var request = new MyFeatureRequest();
        var handler = new MyFeatureHandler();
        
        var response = await handler.Handle(request, CancellationToken.None);

        throw new NotImplementedException("Assert a behaviour of your handler");
    }
}