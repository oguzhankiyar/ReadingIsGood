using OK.ReadingIsGood.Shared.Persistence.Base;

namespace OK.ReadingIsGood.Order.Persistence.Entities
{
    public class OrderItemEntity : EntityBase
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        public virtual OrderEntity Order { get; set; }
    }
}