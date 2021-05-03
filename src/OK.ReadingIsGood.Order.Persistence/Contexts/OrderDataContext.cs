using System.Security.Principal;
using Microsoft.EntityFrameworkCore;
using OK.ReadingIsGood.Order.Persistence.Constants;
using OK.ReadingIsGood.Order.Persistence.Entities;
using OK.ReadingIsGood.Order.Persistence.EntityConfigs;
using OK.ReadingIsGood.Shared.Persistence.Contexts;

namespace OK.ReadingIsGood.Order.Persistence.Contexts
{
    public class OrderDataContext : DataContextBase
    {
        protected override string Schema => TableConstants.SchemaName;

        public virtual DbSet<OrderEntity> Orders { get; set; }
        public virtual DbSet<OrderItemEntity> OrderItems { get; set; }
        public virtual DbSet<OrderStatusEntity> OrderStatuses { get; set; }

        public OrderDataContext(IPrincipal principal, DbContextOptions<OrderDataContext> options) : base(principal, options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new OrderEntityConfig());
            modelBuilder.ApplyConfiguration(new OrderItemEntityConfig());
            modelBuilder.ApplyConfiguration(new OrderStatusEntityConfig());
        }
    }
}