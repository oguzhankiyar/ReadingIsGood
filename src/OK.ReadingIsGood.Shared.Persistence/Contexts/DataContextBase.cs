using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OK.ReadingIsGood.Shared.Persistence.Domain;
using OK.ReadingIsGood.Shared.Persistence.Entities;
using OK.ReadingIsGood.Shared.Persistence.EntityConfigs;

namespace OK.ReadingIsGood.Shared.Persistence.Contexts
{
    public abstract class DataContextBase : DbContext
    {
        protected abstract string Schema { get; }

        public virtual DbSet<ChangesetEntity> Changesets { get; set; }
        public virtual DbSet<ChangeEntity> Changes { get; set; }

        private readonly IPrincipal _principal;

        protected DataContextBase(IPrincipal principal, DbContextOptions options) : base(options)
        {
            _principal = principal;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new ChangesetEntityConfig(Schema));
            modelBuilder.ApplyConfiguration(new ChangeEntityConfig(Schema));
        }

        public override int SaveChanges()
        {
            return SetAndSaveChanges(true).Result;
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            return SetAndSaveChanges(acceptAllChangesOnSuccess).Result;
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return SetAndSaveChanges(true, cancellationToken);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            return SetAndSaveChanges(acceptAllChangesOnSuccess, cancellationToken);
        }

        private async Task<int> SetAndSaveChanges(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            var changesets = new List<ChangesetEntity>();

            var allEntries = ChangeTracker.Entries();

            var newEntries = allEntries.Where(w => w.State == EntityState.Added).ToList();
            var modifiedEntries = allEntries.Where(w => w.State == EntityState.Modified).ToList();
            var deletedEntries = allEntries.Where(w => w.State == EntityState.Deleted).ToList();

            foreach (var entry in newEntries)
            {
                if (entry.Entity is ICreatable creatableEntity)
                {
                    creatableEntity.CreatedAt = DateTime.UtcNow;
                    creatableEntity.CreatedBy = GetUserIdentityName();
                }

                AddChangeset(changesets, "Create", entry);
            }

            foreach (var entry in modifiedEntries)
            {
                if (entry.Entity is IUpdatable updatableEntity)
                {
                    updatableEntity.UpdatedAt = DateTime.UtcNow;
                    updatableEntity.UpdatedBy = GetUserIdentityName();
                }

                if (entry.Entity is IDeletable deletableEntity && deletableEntity.IsDeleted)
                {
                    deletableEntity.DeletedAt = DateTime.UtcNow;
                    deletableEntity.DeletedBy = GetUserIdentityName();
                }

                AddChangeset(changesets, "Update", entry);
            }

            foreach (var entry in deletedEntries)
            {
                if (entry.Entity is IDeletable deletableEntity)
                {
                    entry.State = EntityState.Modified;

                    deletableEntity.IsDeleted = true;
                    deletableEntity.DeletedAt = DateTime.UtcNow;
                    deletableEntity.DeletedBy = GetUserIdentityName();
                }

                AddChangeset(changesets, "Delete", entry);
            }

            var result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);

            if (changesets.Any())
            {
                await Changesets.AddRangeAsync(changesets);
                await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
            }

            return result;
        }

        private void AddChangeset(List<ChangesetEntity> changesets, string operation, EntityEntry entry)
        {
            if (!(entry.Entity is IAuditable))
            {
                return;
            }

            string key = null;

            var prop = entry.Metadata.FindPrimaryKey().Properties.FirstOrDefault();
            if (prop != null)
            {
                key = entry.Property(prop.Name).CurrentValue.ToString();
            }

            var changeset = new ChangesetEntity
            {
                Operation = operation,
                EntityId = key,
                TableName = entry.Metadata.GetTableName(),
                ChangedAt = DateTime.UtcNow,
                ChangedBy = GetUserIdentityName(),
                Changes = new List<ChangeEntity>()
            };

            var excludedProps = new[] {
                nameof(IIdentifiable<int>.Id),
                nameof(ICreatable.CreatedAt),
                nameof(ICreatable.CreatedBy),
                nameof(IUpdatable.UpdatedAt),
                nameof(IUpdatable.UpdatedBy),
                nameof(IDeletable.IsDeleted),
                nameof(IDeletable.DeletedAt),
                nameof(IDeletable.DeletedBy)
            };

            foreach (var property in entry.Properties.Where(x => !excludedProps.Contains(x.Metadata.Name)))
            {
                changeset.Changes.Add(new ChangeEntity
                {
                    PropertyName = property.Metadata.Name,
                    OldValue = property.OriginalValue?.ToString(),
                    NewValue = property.CurrentValue?.ToString()
                });
            }

            changesets.Add(changeset);
        }

        private string GetUserIdentityName() => _principal?.Identity?.Name ?? "unknown";
    }
}