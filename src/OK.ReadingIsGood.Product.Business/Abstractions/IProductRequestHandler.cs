using MediatR;
using OK.ReadingIsGood.Product.Contracts.Requests;
using OK.ReadingIsGood.Product.Contracts.Responses;

namespace OK.ReadingIsGood.Product.Business.Abstractions
{
    public interface IProductRequestHandler :
        IRequestHandler<ProductListRequest, ProductListResponse>,
        IRequestHandler<ProductCreateRequest, ProductCreateResponse>,
        IRequestHandler<ProductEditRequest, ProductEditResponse>
    {
    }
}