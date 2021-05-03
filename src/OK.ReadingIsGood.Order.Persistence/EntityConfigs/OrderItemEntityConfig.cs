using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OK.ReadingIsGood.Order.Persistence.Constants;
using OK.ReadingIsGood.Order.Persistence.Entities;

namespace OK.ReadingIsGood.Order.Persistence.EntityConfigs
{
    public class OrderItemEntityConfig : IEntityTypeConfiguration<OrderItemEntity>
    {
        public void Configure(EntityTypeBuilder<OrderItemEntity> builder)
        {
            builder
                .ToTable(TableConstants.OrderItemTableName, TableConstants.SchemaName)
                .HasKey(x => x.Id);

            builder
                .Property(x => x.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();

            builder
                .HasQueryFilter(x => !x.IsDeleted);

            builder
                .Property(x => x.OrderId)
                .IsRequired();

            builder
                .Property(x => x.ProductId)
                .IsRequired();

            builder
                .Property(x => x.Quantity)
                .IsRequired();

            builder
                .HasOne(x => x.Order)
                .WithMany(x => x.Items)
                .HasForeignKey(x => x.OrderId);
        }
    }
}