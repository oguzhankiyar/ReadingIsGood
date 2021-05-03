using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OK.ReadingIsGood.Product.Persistence.Constants;
using OK.ReadingIsGood.Product.Persistence.Entities;

namespace OK.ReadingIsGood.Product.Persistence.EntityConfigs
{
    public class ProductEntityConfig : IEntityTypeConfiguration<ProductEntity>
    {
        public void Configure(EntityTypeBuilder<ProductEntity> builder)
        {
            builder
                .ToTable(TableConstants.ProductTableName, TableConstants.SchemaName)
                .HasKey(x => x.Id);

            builder
                .Property(x => x.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();

            builder
                .HasQueryFilter(x => !x.IsDeleted);

            builder
                .Property(x => x.Name)
                .IsRequired();

            builder
                .Property(x => x.StockCount)
                .IsRequired();
        }
    }
}