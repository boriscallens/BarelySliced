using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BarelySliced.Persistence
{
    public static class PersistenceServiceCollectionExtensions
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<SliverDbContext>(options =>
            {
                var connectionString = configuration.GetConnectionString(nameof(SliverDbContext));
                if (string.IsNullOrWhiteSpace(connectionString))
                {
                    throw new ArgumentException("Connection string cannot be null or empty.", nameof(configuration));
                }
                options.UseSqlServer(connectionString);
                
            });
            services.AddHealthChecks().AddDbContextCheck<SliverDbContext>();
            return services;
        }

        public static IServiceCollection AddInMemoryPersistence(this IServiceCollection services)
        {
            services.AddDbContext<SliverDbContext>(options =>
            {
                options.UseSqlite("Data Source=:memory:");
            });
            services.AddHealthChecks().AddDbContextCheck<SliverDbContext>();
            return services;
        }
    }
}