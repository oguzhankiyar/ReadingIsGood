using System.Threading;
using System.Threading.Tasks;

namespace OK.ReadingIsGood.Shared.MessageBus.Abstractions
{
    public interface IMessageBus
    {
        Task PublishAsync<T>(T message, CancellationToken cancellationToken = default);
    }
}