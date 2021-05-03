using OK.ReadingIsGood.Shared.Persistence.Base;

namespace OK.ReadingIsGood.Product.Persistence.Entities
{
    public class ProductEntity : EntityBase
    {
        public string Name { get; set; }
        public int StockCount { get; set; }
    }
}