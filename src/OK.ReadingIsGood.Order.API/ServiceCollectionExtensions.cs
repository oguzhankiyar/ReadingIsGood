using Microsoft.Extensions.DependencyInjection;
using OK.ReadingIsGood.Order.API.Attributes;
using OK.ReadingIsGood.Order.API.Config;

namespace OK.ReadingIsGood.Order.API
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddOrderAPI(this IServiceCollection services, OrderAPIConfig config)
        {
            services.AddSingleton(config);

            PathRouteAttribute.Config = config;

            return services;
        }
    }
}