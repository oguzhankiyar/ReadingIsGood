using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OK.ReadingIsGood.Order.Business.Abstractions;
using OK.ReadingIsGood.Order.Contracts.Enums;
using OK.ReadingIsGood.Order.Contracts.Requests;
using OK.ReadingIsGood.Order.Contracts.Responses;
using OK.ReadingIsGood.Order.Persistence.Contexts;
using OK.ReadingIsGood.Order.Persistence.Entities;
using OK.ReadingIsGood.Shared.Core.Events.Order;
using OK.ReadingIsGood.Shared.Core.Exceptions;
using OK.ReadingIsGood.Shared.Core.Extensions;
using OK.ReadingIsGood.Shared.MessageBus.Abstractions;

namespace OK.ReadingIsGood.Order.Business.Handlers
{
    public class OrderRequestHandler : IOrderRequestHandler
    {
        private readonly OrderDataContext _context;
        private readonly IMessageBus _messageBus;
        private readonly IMapper _mapper;

        public OrderRequestHandler(
            OrderDataContext context,
            IMessageBus messageBus,
            IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _messageBus = messageBus ?? throw new ArgumentNullException(nameof(messageBus));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<OrderListResponse> Handle(OrderListRequest request, CancellationToken cancellationToken)
        {
            var query = _context.Orders
                .Include(x => x.Items)
                .AsQueryable();

            if (request.UserId.HasValue)
            {
                query = query.Where(x => x.UserId == request.UserId.Value);
            }

            var data = await query.Sort(request.Sort, request.Order)
                .Paginate(request.PageNumber, request.PageSize, out int pageCount, out int totalCount)
                .ToListAsync(cancellationToken);

            var response = _mapper.Map<OrderListResponse>(data);
            response.PageSize = request.PageSize;
            response.PageNumber = request.PageNumber;
            response.TotalCount = totalCount;
            response.PageCount = pageCount;
            return response;
        }

        public async Task<OrderDetailResponse> Handle(OrderDetailRequest request, CancellationToken cancellationToken)
        {
            var data = await _context.Orders
                .Include(x => x.Items)
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            if (data == null)
            {
                throw new ResourceNotFoundException();
            }

            return _mapper.Map<OrderDetailResponse>(data);
        }

        public async Task<OrderCreateResponse> Handle(OrderCreateRequest request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<OrderEntity>(request);

            entity.StatusId = (int)OrderStatusEnum.Created;

            await _context.Orders.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            var message = _mapper.Map<OrderCreatedEvent>(entity);
            await _messageBus.PublishAsync(message, cancellationToken);

            return _mapper.Map<OrderCreateResponse>(entity);
        }
    }
}