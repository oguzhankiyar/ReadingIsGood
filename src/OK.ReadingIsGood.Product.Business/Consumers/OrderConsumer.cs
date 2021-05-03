using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OK.ReadingIsGood.Product.Persistence.Contexts;
using OK.ReadingIsGood.Shared.Core.Events.Order;
using OK.ReadingIsGood.Shared.MessageBus.Abstractions;

namespace OK.ReadingIsGood.Product.Business.Consumers
{
    public class OrderConsumer : IMessageConsumer<OrderCreatedEvent>
    {
        private readonly ProductDataContext _context;
        private readonly ILogger<OrderConsumer> _logger;

        public OrderConsumer(
            ProductDataContext context,
            ILogger<OrderConsumer> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task ConsumeAsync(OrderCreatedEvent message, CancellationToken cancellationToken = default)
        {
            var productMap = message.Order.Items.ToDictionary(x => x.ProductId, x => x.Quantity);
            var productIds = productMap.Keys;
            var products = await _context.Products
                .Where(x => productIds.Contains(x.Id))
                .ToListAsync(cancellationToken);

            foreach (var item in productMap)
            {
                var product = products.FirstOrDefault(x => x.Id == item.Key);
                if (product == null)
                {
                    continue;
                }

                if (product.StockCount == item.Value)
                {
                    product.StockCount = 0;
                }
                else if (product.StockCount < item.Value)
                {
                    _logger.LogDebug($"The product quantity is insufficent while order creating! | ProductId: {product.Id} | OrderId: {message.Order.Id}");

                    product.StockCount = 0;
                }
                else
                {
                    product.StockCount -= item.Value;
                }
            }

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}