using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OK.ReadingIsGood.Shared.Persistence.Constants;
using OK.ReadingIsGood.Shared.Persistence.Entities;

namespace OK.ReadingIsGood.Shared.Persistence.EntityConfigs
{
    public class ChangeEntityConfig : IEntityTypeConfiguration<ChangeEntity>
    {
        private readonly string _schema;

        public ChangeEntityConfig(string schema)
        {
            _schema = schema ?? throw new ArgumentNullException(nameof(schema));
        }

        public void Configure(EntityTypeBuilder<ChangeEntity> builder)
        {
            builder
                .ToTable(TableConstants.ChangeTableName, _schema)
                .HasKey(x => x.Id);

            builder
                .Property(x => x.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();

            builder
                .Property(x => x.PropertyName)
                .IsRequired();

            builder
                .Property(x => x.OldValue)
                .IsRequired(false);

            builder
                .Property(x => x.NewValue)
                .IsRequired(false);

            builder
                .HasOne(x => x.Changeset)
                .WithMany(x => x.Changes)
                .HasForeignKey(x => x.ChangesetId);
        }
    }
}