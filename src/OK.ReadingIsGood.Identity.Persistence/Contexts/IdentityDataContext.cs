using System.Security.Principal;
using Microsoft.EntityFrameworkCore;
using OK.ReadingIsGood.Identity.Persistence.Constants;
using OK.ReadingIsGood.Identity.Persistence.Entities;
using OK.ReadingIsGood.Identity.Persistence.EntityConfigs;
using OK.ReadingIsGood.Shared.Persistence.Contexts;

namespace OK.ReadingIsGood.Identity.Persistence.Contexts
{
    public class IdentityDataContext : DataContextBase
    {
        protected override string Schema => TableConstants.SchemaName;

        public virtual DbSet<UserEntity> Users { get; set; }

        public IdentityDataContext(IPrincipal principal, DbContextOptions<IdentityDataContext> options) : base(principal, options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new UserEntityConfig());
        }
    }
}