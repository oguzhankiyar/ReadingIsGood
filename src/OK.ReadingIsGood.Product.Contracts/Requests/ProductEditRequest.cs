using MediatR;
using OK.ReadingIsGood.Product.Contracts.Responses;

namespace OK.ReadingIsGood.Product.Contracts.Requests
{
    public class ProductEditRequest : IRequest<ProductEditResponse>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int StockCount { get; set; }
    }
}