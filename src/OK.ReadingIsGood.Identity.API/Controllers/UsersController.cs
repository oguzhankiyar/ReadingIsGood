using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OK.ReadingIsGood.Identity.API.Base;
using OK.ReadingIsGood.Identity.Contracts.Requests;
using OK.ReadingIsGood.Identity.Contracts.Responses;

namespace OK.ReadingIsGood.Identity.API.Controllers
{
    public class UsersController : BaseController
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [Authorize]
        [HttpGet]
        public Task<UserListResponse> GetAsync([FromQuery] UserListRequest request, CancellationToken cancellationToken)
        {
            return _mediator.Send(request, cancellationToken);
        }

        [HttpPost]
        public Task<UserCreateResponse> PostAsync([FromBody] UserCreateRequest request, CancellationToken cancellationToken)
        {
            return _mediator.Send(request, cancellationToken);
        }
    }
}