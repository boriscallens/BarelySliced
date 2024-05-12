using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

using Xunit.Abstractions;

namespace BarelySliced.Persistence.Tests;

public class PersistenceServiceCollectionExtensionsShould
{
    private readonly ITestOutputHelper output;

    public PersistenceServiceCollectionExtensionsShould(ITestOutputHelper output)
    {
        this.output = output;
    }

    [Fact]
    public void AddPersistenceServices()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryConnectionString("SliverDbContext", "Server=(localdb)\\MSSQLLocalDB;Database=SliverD;Trusted_Connection=True;")
            .Build();

        var services = new ServiceCollection().AddPersistence(configuration);

        var serviceProvider = services.BuildServiceProvider();
        var dbContext = serviceProvider.GetRequiredService<SliverDbContext>();
        Assert.NotNull(dbContext);
    }

    [Fact]
    public void NotAllowMissingConnectionString()
    {
        var configuration = new ConfigurationBuilder().Build();

        // Act
        var services = new ServiceCollection().AddPersistence(configuration);

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        Assert.Throws<ArgumentException>(() => serviceProvider.GetRequiredService<SliverDbContext>());
    }

    [Fact]
    public void AddHealthCheck()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryConnectionString("SliverDbContext", "Server=(localdb)\\MSSQLLocalDB;Database=SliverD;Trusted_Connection=True;")
            .Build();

        var services = new ServiceCollection()
            .AddLogging()
            .AddPersistence(configuration);

        var serviceProvider = services.BuildServiceProvider();
        var healthCheck = serviceProvider.GetRequiredService<HealthCheckService>();
        Assert.NotNull(healthCheck);
    }

    [Fact]
    public async Task PassesHealthCheckForValidConnectionString()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryConnectionString("SliverDbContext", "Server=(localdb)\\MSSQLLocalDB;Database=SliverDB;Trusted_Connection=True;")
            .Build();

        var services = new ServiceCollection()
            .AddLogging()
            .AddPersistence(configuration);

        var serviceProvider = services.BuildServiceProvider();
        var healthCheck = serviceProvider.GetRequiredService<HealthCheckService>();
        var result = await healthCheck.CheckHealthAsync();

        // Log the failure details
        var failedChecks = result.Entries
            .Where(e => e.Key.Equals(nameof(SliverDbContext)))
            .Where(e => e.Value.Status != HealthStatus.Healthy);

        foreach (var entry in failedChecks)
        {
            // Log the name and description of the failed health check
            output.WriteLine($"Health check '{entry.Key}' failed: '{entry.Value.Description}'. Make sure the database as defined in the connection string exists and is available.");

            // Log the exception, if any
            if (entry.Value.Exception != null)
            {
                output.WriteLine($"Exception: {entry.Value.Exception}");
            }
        }

        Assert.Equal(HealthStatus.Healthy, result.Status);
    }

    [Fact]
    public async Task FailsHealthCheckForBadConnectionString()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryConnectionString("SliverDbContext", "nonsense")
            .Build();

        var services = new ServiceCollection()
            .AddLogging()
            .AddPersistence(configuration);

        var serviceProvider = services.BuildServiceProvider();
        var healthCheck = serviceProvider.GetRequiredService<HealthCheckService>();
        var result = await healthCheck.CheckHealthAsync();

        Assert.NotEqual(HealthStatus.Healthy, result.Status);
    }
}
