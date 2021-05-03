using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OK.ReadingIsGood.Shared.MessageBus.Abstractions;
using OK.ReadingIsGood.Shared.MessageBus.Config;
using OK.ReadingIsGood.Shared.MessageBus.InMemory;

namespace OK.ReadingIsGood.Shared.MessageBus
{
    public static class ServiceCollectionExtensions
    {
        private static MessageBusConfig _config;

        public static IServiceCollection AddMessageBus(this IServiceCollection services, Action<MessageBusConfig> configAction = null)
        {
            var config = new MessageBusConfig();
            if (configAction != null)
            {
                configAction.Invoke(config);
            }

            if (_config != null)
            {
                _config.Consumers.AddRange(config.Consumers);
            }
            else
            {
                _config = config;
            }

            services.RemoveAll(typeof(MessageBusConfig));
            services.AddSingleton(_config);

            return services;
        }

        public static IServiceCollection AddInMemoryMessageBus(this IServiceCollection services)
        {
            services.AddSingleton<IMessageBus, InMemoryMessageBus>();

            return services;
        }
    }
}