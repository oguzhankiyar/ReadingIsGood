using System;
using System.Collections.Generic;
using System.Linq;
using OK.ReadingIsGood.Shared.MessageBus.Abstractions;

namespace OK.ReadingIsGood.Shared.MessageBus.Config
{
    public class MessageBusConfig
    {
        public List<(Type MessageType, Type ConsumerType)> Consumers { get; set; }

        public MessageBusConfig()
        {
            Consumers = new List<(Type MessageType, Type ConsumerType)>();
        }

        public MessageBusConfig AddConsumer<T>() where T : class
        {
            var consumerType = typeof(T);
            var interfaceTypes = consumerType.GetInterfaces().Where(x => x.Name == typeof(IMessageConsumer<>).Name);

            foreach (var interfaceType in interfaceTypes)
            {
                var messageType = interfaceType.GenericTypeArguments[0];

                Consumers.Add((messageType, consumerType));
            }

            return this;
        }
    }
}