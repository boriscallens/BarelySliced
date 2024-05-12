using Microsoft.Extensions.DependencyInjection;

namespace BarelySliced.Persistence.Tests;

public class UnitTestFixture : IServiceProvider, IDisposable
{
    private readonly ServiceProvider _provider;

    public UnitTestFixture()
    {
        var serviceCollection = new ServiceCollection();
        _provider = serviceCollection.BuildServiceProvider();
    }

    public object? GetService(Type serviceType)
    {
        return _provider.GetService(serviceType);
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        _provider.Dispose();
    }
}