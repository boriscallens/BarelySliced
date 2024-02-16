using BarelySliced.Persistence;

using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BarelySliced.Business.Tests;

public static class PersistenceServiceCollectionExtensions
{
    public static void AddInMemoryPersistence(this IServiceCollection services, bool isTransient = true)
    {
        static SliverDBContext SqlLiteInMemoryFactory(IServiceProvider provider)
        {
            var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();
            var options = new DbContextOptionsBuilder<SliverDBContext>()
                .UseSqlite(connection)
                .EnableDetailedErrors()
                .EnableSensitiveDataLogging()
                .Options;
            var context = new SliverDBContext(options);
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
