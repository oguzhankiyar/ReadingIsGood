using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using OK.ReadingIsGood.Shared.MessageBus.Abstractions;
using OK.ReadingIsGood.Shared.MessageBus.Config;

namespace OK.ReadingIsGood.Shared.MessageBus.InMemory
{
    public class InMemoryMessageBus : IMessageBus
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly MessageBusConfig _config;

        public InMemoryMessageBus(
            IServiceProvider serviceProvider,
            MessageBusConfig config)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public async Task PublishAsync<T>(T message, CancellationToken cancellationToken = default)
        {
            var consumerTypes = _config.Consumers.Where(x => x.MessageType == typeof(T));

            foreach (var consumerType in consumerTypes)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return;
                }

                var consumer = _serviceProvider.CreateScope().ServiceProvider.GetService(consumerType.ConsumerType) as IMessageConsumer<T>;
                if (consumer != null)
                {
                    await consumer.ConsumeAsync(message, cancellationToken);
                }
            }
        }
    }
}