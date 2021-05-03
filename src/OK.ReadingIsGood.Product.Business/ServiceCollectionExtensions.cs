using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using OK.ReadingIsGood.Product.Business.Behaviors;
using OK.ReadingIsGood.Product.Business.Config;
using OK.ReadingIsGood.Product.Business.Consumers;
using OK.ReadingIsGood.Shared.MessageBus;

namespace OK.ReadingIsGood.Product.Business
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddProductBusiness(this IServiceCollection services, ProductBusinessConfig config)
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

            services.AddTransient<OrderConsumer>();

            services.AddMessageBus(x =>
            {
                x.AddConsumer<OrderConsumer>();
            });

            return services;
        }
    }
}