using BarelySliced.Domain;

using Microsoft.OpenApi.Models;

namespace BarelySliced.Api
{
    public static class OpenApiServiceCollectionExtensions
    {
        public static void AddOpenApi(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "BarelySliced.Api", Version = "v1" });

                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    BearerFormat = "JWT",
                    Flows = new OpenApiOAuthFlows
                    {
                        Implicit = new OpenApiOAuthFlow
                        {
                            TokenUrl = new Uri($"https://{configuration["Auth0:Domain"]}/oauth/token"),
                            AuthorizationUrl = new Uri($"https://{configuration["Auth0:Domain"]}/authorize?audience={configuration["Auth0:Audience"]}"),
                            Scopes = new Dictionary<string, string>
                            {
                                { Policies.ManageWeatherForecast, "Read access to protected weather data" }
                            }
                        }
                    }
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" }
                        },
                        new[] { Policies.ManageWeatherForecast }
                    }
                });

            });
        }

        public static void UseOpenApi(this IApplicationBuilder app, IConfiguration configuration)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.OAuthClientId(configuration["Auth0:ClientId"]);
                options.OAuthClientSecret(configuration["Auth0:ClientSecret"]);
                options.OAuthRealm(configuration["Auth0:Realm"]);
                options.OAuthAppName(configuration["Auth0:AppName"]);
                options.OAuthScopeSeparator(" ");
                options.OAuthUsePkce();
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "BarelySliced.Api v1");
            });
        }
    }
}
