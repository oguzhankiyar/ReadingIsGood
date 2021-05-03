using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OK.ReadingIsGood.Shared.Persistence.Constants;
using OK.ReadingIsGood.Shared.Persistence.Entities;

namespace OK.ReadingIsGood.Shared.Persistence.EntityConfigs
{
    public class ChangesetEntityConfig : IEntityTypeConfiguration<ChangesetEntity>
    {
        private readonly string _schema;

        public ChangesetEntityConfig(string schema)
        {
            _schema = schema ?? throw new ArgumentNullException(nameof(schema));
        }

        public void Configure(EntityTypeBuilder<ChangesetEntity> builder)
        {
            builder
                .ToTable(TableConstants.ChangesetTableName, _schema)
                .HasKey(x => x.Id);

            builder
                .Property(x => x.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();

            builder
                .Property(x => x.Operation)
                .IsRequired();

            builder
                .Property(x => x.TableName)
                .IsRequired();

            builder
                .Property(x => x.EntityId)
                .IsRequired();

            builder
                .Property(x => x.ChangedBy)
                .IsRequired();

            builder
                .Property(x => x.ChangedAt)
                .IsRequired();
        }
    }
}