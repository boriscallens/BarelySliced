using System.Security.Claims;

using BarelySliced.Business.Authorization;
using BarelySliced.Domain;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace BarelySliced.Infrastructure
{
    public static class AuthServiceCollectionExtensions
    {
        public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration config)
        {
            string domain = $"https://{config["Auth0:Domain"]}/";
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = domain;
                    options.Audience = config["Auth0:Audience"];
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        NameClaimType = ClaimTypes.NameIdentifier
                    };
                });

            services.AddAuthorizationBuilder()
                .AddPolicy(Policies.ManageWeatherForecast, policy => policy.Requirements.Add(new HasScopeRequirement(Policies.ManageWeatherForecast, domain)));

            services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();

            return services;
        }
    }
}
