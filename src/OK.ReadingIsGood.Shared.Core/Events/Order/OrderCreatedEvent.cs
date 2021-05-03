using System.Collections.Generic;

namespace OK.ReadingIsGood.Shared.Core.Events.Order
{
    public class OrderCreatedEvent
    {
        public class OrderModel
        {
            public int Id { get; set; }
            public int UserId { get; set; }
            public List<OrderItemModel> Items { get; set; } = new List<OrderItemModel>();
        }

        public class OrderItemModel
        {
            public int ProductId { get; set; }
            public int Quantity { get; set; }
        }

        public OrderModel Order { get; set; }
    }
}