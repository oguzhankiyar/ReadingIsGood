using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using OK.ReadingIsGood.Order.Business.Behaviors;
using OK.ReadingIsGood.Order.Business.Config;

namespace OK.ReadingIsGood.Order.Business
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddOrderBusiness(this IServiceCollection services, OrderBusinessConfig config)
        {
            services.AddSingleton(config);

            var assemblies = new Assembly[]
            {
                Assembly.GetExecutingAssembly()
            };

            services.Add(new ServiceDescriptor(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>), ServiceLifetime.Transient));

            services.AddMediatR(assemblies);
            services.AddAutoMapper(assemblies);
            services.AddValidatorsFromAssemblies(assemblies, ServiceLifetime.Transient);

            return services;
        }
    }
}