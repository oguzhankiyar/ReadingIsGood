using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OK.ReadingIsGood.Identity.Business.Abstractions;
using OK.ReadingIsGood.Identity.Business.Helpers;
using OK.ReadingIsGood.Identity.Contracts.Requests;
using OK.ReadingIsGood.Identity.Contracts.Responses;
using OK.ReadingIsGood.Identity.Persistence.Contexts;
using OK.ReadingIsGood.Identity.Persistence.Entities;
using OK.ReadingIsGood.Shared.Core.Events.User;
using OK.ReadingIsGood.Shared.Core.Exceptions;
using OK.ReadingIsGood.Shared.Core.Extensions;
using OK.ReadingIsGood.Shared.MessageBus.Abstractions;

namespace OK.ReadingIsGood.Identity.Business.Handlers
{
    public class UserRequestHandler : IUserRequestHandler
    {
        private readonly IdentityDataContext _context;
        private readonly IPasswordHelper _passwordHelper;
        private readonly IMessageBus _messageBus;
        private readonly IMapper _mapper;

        public UserRequestHandler(
            IdentityDataContext context,
            IPasswordHelper passwordHelper,
            IMessageBus messageBus,
            IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _passwordHelper = passwordHelper ?? throw new ArgumentNullException(nameof(passwordHelper));
            _messageBus = messageBus ?? throw new ArgumentNullException(nameof(messageBus));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<UserListResponse> Handle(UserListRequest request, CancellationToken cancellationToken)
        {
            var data = await _context.Users
                .Sort(request.Sort, request.Order)
                .Paginate(request.PageNumber, request.PageSize, out int pageCount, out int totalCount)
                .ToListAsync(cancellationToken);

            var response = _mapper.Map<UserListResponse>(data);
            response.PageSize = request.PageSize;
            response.PageNumber = request.PageNumber;
            response.TotalCount = totalCount;
            response.PageCount = pageCount;
            return response;
        }

        public async Task<UserCreateResponse> Handle(UserCreateRequest request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<UserEntity>(request);

            entity.Password = _passwordHelper.Hash(entity.Password);

            var isExist = await _context.Users
                .AnyAsync(x => x.Email == request.Email, cancellationToken);
            if (isExist)
            {
                throw new RequestNotValidatedException("The user email was already taken.");
            }

            await _context.Users.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            var message = _mapper.Map<UserCreatedEvent>(entity);
            await _messageBus.PublishAsync(message);

            return _mapper.Map<UserCreateResponse>(entity);
        }
    }
}