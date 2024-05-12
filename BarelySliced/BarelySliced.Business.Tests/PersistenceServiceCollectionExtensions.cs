using BarelySliced.Persistence;

using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BarelySliced.Business.Tests;

public static class PersistenceServiceCollectionExtensions
{
    public static void AddInMemoryPersistence(this IServiceCollection services, bool isTransient = true)
    {
        static SliverDbContext SqlLiteInMemoryFactory(IServiceProvider provider)
        {
            var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();
            var options = new DbContextOptionsBuilder<SliverDbContext>()
                .UseSqlite(connection)
                .EnableDetailedErrors()
                .EnableSensitiveDataLogging()
                .Options;
            var context = new SliverDbContext(options);
            context.Database.EnsureCreated();
            return context;
        }
        if (isTransient)
        {
            services.AddTransient(SqlLiteInMemoryFactory);
        }
        else
        {
            services.AddSingleton(SqlLiteInMemoryFactory);
        }
    }
}
