using Microsoft.Extensions.DependencyInjection;
using OK.ReadingIsGood.Identity.API.Attributes;
using OK.ReadingIsGood.Identity.API.Config;

namespace OK.ReadingIsGood.Identity.API
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIdentityAPI(this IServiceCollection services, IdentityAPIConfig config)
        {
            services.AddSingleton(config);

            PathRouteAttribute.Config = config;

            return services;
        }
    }
}