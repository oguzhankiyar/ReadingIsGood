using System;
using System.Collections.Generic;
using OK.ReadingIsGood.Order.Contracts.Enums;

namespace OK.ReadingIsGood.Order.Contracts.Models
{
    public class OrderModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int StatusId { get; set; }
        public List<OrderItemModel> Items { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }

        public OrderModel()
        {
            Items = new List<OrderItemModel>();
        }
    }
}