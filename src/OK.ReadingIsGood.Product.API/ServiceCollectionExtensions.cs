using Microsoft.Extensions.DependencyInjection;
using OK.ReadingIsGood.Product.API.Attributes;
using OK.ReadingIsGood.Product.API.Config;

namespace OK.ReadingIsGood.Product.API
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddProductAPI(this IServiceCollection services, ProductAPIConfig config)
        {
            services.AddSingleton(config);

            PathRouteAttribute.Config = config;

            return services;
        }
    }
}