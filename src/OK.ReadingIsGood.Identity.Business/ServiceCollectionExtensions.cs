using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using OK.ReadingIsGood.Identity.Business.Behaviors;
using OK.ReadingIsGood.Identity.Business.Config;
using OK.ReadingIsGood.Identity.Business.Helpers;

namespace OK.ReadingIsGood.Identity.Business
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIdentityBusiness(this IServiceCollection services, IdentityBusinessConfig config)
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

            services.AddSingleton<IPasswordHelper, PasswordHelper>();

            return services;
        }
    }
}