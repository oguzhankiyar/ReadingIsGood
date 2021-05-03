using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OK.ReadingIsGood.Order.Contracts.Enums;
using OK.ReadingIsGood.Order.Persistence.Constants;
using OK.ReadingIsGood.Order.Persistence.Entities;

namespace OK.ReadingIsGood.Order.Persistence.EntityConfigs
{
    public class OrderStatusEntityConfig : IEntityTypeConfiguration<OrderStatusEntity>
    {
        public void Configure(EntityTypeBuilder<OrderStatusEntity> builder)
        {
            builder
                .ToTable(TableConstants.OrderStatusTableName, TableConstants.SchemaName)
                .HasKey(x => x.Id);

            builder
                .Property(x => x.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();

            builder
                .Property(x => x.Name)
                .IsRequired();

            builder
                .HasData(
                    new OrderStatusEntity { Id = (int)OrderStatusEnum.Created, Name = OrderStatusEnum.Created.ToString() },
                    new OrderStatusEntity { Id = (int)OrderStatusEnum.Prepared, Name = OrderStatusEnum.Prepared.ToString() },
                    new OrderStatusEntity { Id = (int)OrderStatusEnum.Shipped, Name = OrderStatusEnum.Shipped.ToString() },
                    new OrderStatusEntity { Id = (int)OrderStatusEnum.Cancelled, Name = OrderStatusEnum.Cancelled.ToString() }
                );
        }
    }
}