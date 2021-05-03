using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OK.ReadingIsGood.Product.API.Base;
using OK.ReadingIsGood.Product.Contracts.Requests;
using OK.ReadingIsGood.Product.Contracts.Responses;

namespace OK.ReadingIsGood.Product.API.Controllers
{
    [Authorize]
    public class ProductsController : BaseController
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet]
        public Task<ProductListResponse> GetAsync([FromQuery] ProductListRequest request, CancellationToken cancellationToken)
        {
            return _mediator.Send(request, cancellationToken);
        }

        [HttpPost]
        public Task<ProductCreateResponse> PostAsync([FromBody] ProductCreateRequest request, CancellationToken cancellationToken)
        {
            return _mediator.Send(request, cancellationToken);
        }

        [HttpPut]
        public Task<ProductEditResponse> PutAsync([FromBody] ProductEditRequest request, CancellationToken cancellationToken)
        {
            return _mediator.Send(request, cancellationToken);
        }
    }
}