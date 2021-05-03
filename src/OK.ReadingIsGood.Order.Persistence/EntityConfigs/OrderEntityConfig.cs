using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OK.ReadingIsGood.Order.Persistence.Constants;
using OK.ReadingIsGood.Order.Persistence.Entities;

namespace OK.ReadingIsGood.Order.Persistence.EntityConfigs
{
    public class OrderEntityConfig : IEntityTypeConfiguration<OrderEntity>
    {
        public void Configure(EntityTypeBuilder<OrderEntity> builder)
        {
            builder
                .ToTable(TableConstants.OrderTableName, TableConstants.SchemaName)
                .HasKey(x => x.Id);

            builder
                .Property(x => x.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();

            builder
                .HasQueryFilter(x => !x.IsDeleted);

            builder
                .Property(x => x.UserId)
                .IsRequired();

            builder
                .Property(x => x.StatusId)
                .IsRequired();

            builder
                .HasOne(x => x.Status)
                .WithMany()
                .HasForeignKey(x => x.StatusId);
        }
    }
}