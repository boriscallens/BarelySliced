using BarelySliced.Business.Features.MyFeature;
using BarelySliced.Persistence;

using Microsoft.Extensions.DependencyInjection;

namespace BarelySliced.Business.Tests.Features.MyFeature;

public class MyFeatureShould: IClassFixture<UnitTestFixture>
{
    private readonly SliverDbContext db;

    public MyFeatureShould(UnitTestFixture fixture)
    {
        db = fixture.GetRequiredService<SliverDbContext>();
    }

    [Fact]
    public async Task DoSomething()
    {
        var request = new MyFeatureRequest();
        var handler = new MyFeatureHandler(db);
        
        var response = await handler.Handle(request, CancellationToken.None);

        throw new NotImplementedException("Assert a behaviour of your handler");
    }
}