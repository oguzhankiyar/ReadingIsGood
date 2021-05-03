using MediatR;
using OK.ReadingIsGood.Product.Contracts.Responses;
using OK.ReadingIsGood.Shared.Core.Requests;

namespace OK.ReadingIsGood.Product.Contracts.Requests
{
    public class ProductListRequest : BaseListRequest, IRequest<ProductListResponse>
    {

    }
}