using System.Security.Principal;
using Microsoft.EntityFrameworkCore;
using OK.ReadingIsGood.Product.Persistence.Constants;
using OK.ReadingIsGood.Product.Persistence.Entities;
using OK.ReadingIsGood.Product.Persistence.EntityConfigs;
using OK.ReadingIsGood.Shared.Persistence.Contexts;

namespace OK.ReadingIsGood.Product.Persistence.Contexts
{
    public class ProductDataContext : DataContextBase
    {
        protected override string Schema => TableConstants.SchemaName;

        public virtual DbSet<ProductEntity> Products { get; set; }

        public ProductDataContext(IPrincipal principal, DbContextOptions<ProductDataContext> options) : base(principal, options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new ProductEntityConfig());
        }
    }
}