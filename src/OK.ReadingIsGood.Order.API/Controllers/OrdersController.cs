using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OK.ReadingIsGood.Order.API.Base;
using OK.ReadingIsGood.Order.Contracts.Requests;
using OK.ReadingIsGood.Order.Contracts.Responses;

namespace OK.ReadingIsGood.Order.API.Controllers
{
    [Authorize]
    public class OrdersController : BaseController
    {
        private readonly IMediator _mediator;

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet]
        public Task<OrderListResponse> GetAsync([FromQuery] OrderListRequest request, CancellationToken cancellationToken)
        {
            return _mediator.Send(request, cancellationToken);
        }

        [HttpGet("{Id}")]
        public Task<OrderDetailResponse> GetAsync([FromRoute] OrderDetailRequest request, CancellationToken cancellationToken)
        {
            return _mediator.Send(request, cancellationToken);
        }

        [HttpPost]
        public Task<OrderCreateResponse> PostAsync([FromBody] OrderCreateRequest request, CancellationToken cancellationToken)
        {
            return _mediator.Send(request, cancellationToken);
        }
    }
}