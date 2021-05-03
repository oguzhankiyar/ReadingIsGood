using System;

namespace OK.ReadingIsGood.Order.Contracts.Models
{
    public class OrderItemModel
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
    }
}