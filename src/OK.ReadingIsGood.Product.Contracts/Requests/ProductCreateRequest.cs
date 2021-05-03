using MediatR;
using OK.ReadingIsGood.Product.Contracts.Responses;

namespace OK.ReadingIsGood.Product.Contracts.Requests
{
    public class ProductCreateRequest : IRequest<ProductCreateResponse>
    {
        public string Name { get; set; }
        public int StockCount { get; set; }
    }
}