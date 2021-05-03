using System.Collections.Generic;
using OK.ReadingIsGood.Shared.Persistence.Base;

namespace OK.ReadingIsGood.Order.Persistence.Entities
{
    public class OrderEntity : EntityBase
    {
        public int UserId { get; set; }
        public int StatusId { get; set; }

        public virtual OrderStatusEntity Status { get; set; }
        public virtual ICollection<OrderItemEntity> Items { get; set; }
    }
}