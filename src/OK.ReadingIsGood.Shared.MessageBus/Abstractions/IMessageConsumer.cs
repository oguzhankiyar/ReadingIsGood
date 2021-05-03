using System.Threading;
using System.Threading.Tasks;

namespace OK.ReadingIsGood.Shared.MessageBus.Abstractions
{
    public interface IMessageConsumer<T>
    {
        Task ConsumeAsync(T message, CancellationToken cancellationToken = default);
    }
}