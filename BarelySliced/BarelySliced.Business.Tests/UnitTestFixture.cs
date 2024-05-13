using BarelySliced.Persistence;

using Microsoft.Extensions.DependencyInjection;

namespace BarelySliced.Business.Tests;

public class UnitTestFixture : IServiceProvider, IDisposable
{
    private readonly ServiceProvider provider;

    public UnitTestFixture()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddInMemoryPersistence();
        provider = serviceCollection.BuildServiceProvider();
    }

    public object? GetService(Type serviceType)
    {
        return provider.GetService(serviceType);
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        provider.Dispose();
    }
}