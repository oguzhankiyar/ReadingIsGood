using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OK.ReadingIsGood.Identity.Persistence.Constants;
using OK.ReadingIsGood.Identity.Persistence.Entities;

namespace OK.ReadingIsGood.Identity.Persistence.EntityConfigs
{
    public class UserEntityConfig : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder
                .ToTable(TableConstants.UserTableName, TableConstants.SchemaName)
                .HasKey(x => x.Id);

            builder
                .Property(x => x.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();

            builder
                .HasQueryFilter(x => !x.IsDeleted);

            builder
                .Property(x => x.FullName)
                .IsRequired();

            builder
                .Property(x => x.Email)
                .IsRequired();

            builder
                .Property(x => x.Password)
                .IsRequired();
        }
    }
}