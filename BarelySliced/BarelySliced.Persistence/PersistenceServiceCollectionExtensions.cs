using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BarelySliced.Persistence
{
    public static class PersistenceServiceCollectionExtensions
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration, bool isDevelopment)
        {
            services.AddDbContext<SliverDbContext>(options =>
            {
                var connectionString = configuration.GetConnectionString(nameof(SliverDbContext));
                if (string.IsNullOrWhiteSpace(connectionString))
                {
                    throw new ArgumentException("Connection string cannot be null or empty.", nameof(configuration));
                }
                options.UseSqlServer(connectionString);
                options.EnableSensitiveDataLogging(isDevelopment);
                options.EnableDetailedErrors(isDevelopment);                
            });
            services.AddHealthChecks().AddDbContextCheck<SliverDbContext>();
            return services;
        }

        public static IServiceCollection AddInMemoryPersistence(this IServiceCollection services, bool isTransient = true)
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
            services.AddHealthChecks().AddDbContextCheck<SliverDbContext>();
            return services;
        }
    }
}