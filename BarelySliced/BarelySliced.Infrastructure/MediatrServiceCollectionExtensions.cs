using Microsoft.Extensions.DependencyInjection;

namespace BarelySliced.Infrastructure
{
    public static class MediatrServiceCollectionExtensions
    {
        public static IServiceCollection AddMediator(this IServiceCollection services)
        {
            var businessAssembly = AppDomain.CurrentDomain.Load("BarelySliced.Business");
            services.AddMediatR(cfg => {
                cfg.RegisterServicesFromAssembly(businessAssembly);
                //cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
                //cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(RequestAuthorizationBehaviour<,>));
            });
            return services;
        }
    }
}
