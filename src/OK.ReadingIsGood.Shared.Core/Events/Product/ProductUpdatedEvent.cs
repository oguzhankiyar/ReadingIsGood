namespace OK.ReadingIsGood.Shared.Core.Events.Product
{
    public class ProductUpdatedEvent
    {
        public class ProductModel
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int StockCount { get; set; }
        }

        public ProductModel Product { get; set; }
    }
}