using Microsoft.Extensions.Configuration;

namespace BarelySliced.Persistence.Tests
{
    public static class ConfigurationBuilderExtensions
    {
        public static IConfigurationBuilder AddInMemoryConnectionString(this IConfigurationBuilder builder, string name, string value)
        {
            Dictionary<string, string?> configs = new() {
                { $"ConnectionStrings:{name}", value }
            };
            builder.AddInMemoryCollection(configs);
            return builder;
        }
    }
}
