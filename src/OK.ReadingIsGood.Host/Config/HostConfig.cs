using OK.ReadingIsGood.Identity.API.Config;
using OK.ReadingIsGood.Identity.Business.Config;
using OK.ReadingIsGood.Identity.Persistence.Config;
using OK.ReadingIsGood.Order.API.Config;
using OK.ReadingIsGood.Order.Business.Config;
using OK.ReadingIsGood.Order.Persistence.Config;
using OK.ReadingIsGood.Product.API.Config;
using OK.ReadingIsGood.Product.Business.Config;
using OK.ReadingIsGood.Product.Persistence.Config;

namespace OK.ReadingIsGood.Host.Config
{
    public class HostConfig
    {
        public class AppConfig
        {
            public string Title { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
        }

        public class ModulesConfig
        {
            public IdentityModuleConfig Identity { get; set; } = new IdentityModuleConfig();
            public ProductModuleConfig Product { get; set; } = new ProductModuleConfig();
            public OrderModuleConfig Order { get; set; } = new OrderModuleConfig();
        }

        public class IdentityModuleConfig
        {
            public IdentityAPIConfig API { get; set; } = new IdentityAPIConfig();
            public IdentityBusinessConfig Business { get; set; } = new IdentityBusinessConfig();
            public IdentityPersistenceConfig Persistence { get; set; } = new IdentityPersistenceConfig();
        }

        public class ProductModuleConfig
        {
            public ProductAPIConfig API { get; set; } = new ProductAPIConfig();
            public ProductBusinessConfig Business { get; set; } = new ProductBusinessConfig();
            public ProductPersistenceConfig Persistence { get; set; } = new ProductPersistenceConfig();
        }

        public class OrderModuleConfig
        {
            public OrderAPIConfig API { get; set; } = new OrderAPIConfig();
            public OrderBusinessConfig Business { get; set; } = new OrderBusinessConfig();
            public OrderPersistenceConfig Persistence { get; set; } = new OrderPersistenceConfig();
        }

        public class CorsConfig
        {
            public string[] Origins { get; set; }
        }

        public AppConfig App { get; set; } = new AppConfig();
        public CorsConfig Cors { get; set; } = new CorsConfig();
        public ModulesConfig Modules { get; set; } = new ModulesConfig();
    }
}