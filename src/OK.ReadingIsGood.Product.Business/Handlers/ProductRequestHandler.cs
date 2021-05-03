using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OK.ReadingIsGood.Product.Business.Abstractions;
using OK.ReadingIsGood.Product.Contracts.Requests;
using OK.ReadingIsGood.Product.Contracts.Responses;
using OK.ReadingIsGood.Product.Persistence.Contexts;
using OK.ReadingIsGood.Product.Persistence.Entities;
using OK.ReadingIsGood.Shared.Core.Events.Product;
using OK.ReadingIsGood.Shared.Core.Exceptions;
using OK.ReadingIsGood.Shared.Core.Extensions;
using OK.ReadingIsGood.Shared.MessageBus.Abstractions;

namespace OK.ReadingIsGood.Product.Business.Handlers
{
    public class ProductRequestHandler : IProductRequestHandler
    {
        private readonly ProductDataContext _context;
        private readonly IMessageBus _messageBus;
        private readonly IMapper _mapper;

        public ProductRequestHandler(
            ProductDataContext context,
            IMessageBus messageBus,
            IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _messageBus = messageBus ?? throw new ArgumentNullException(nameof(messageBus));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<ProductListResponse> Handle(ProductListRequest request, CancellationToken cancellationToken)
        {
            var data = await _context.Products
                .Sort(request.Sort, request.Order)
                .Paginate(request.PageNumber, request.PageSize, out int pageCount, out int totalCount)
                .ToListAsync(cancellationToken);

            var response = _mapper.Map<ProductListResponse>(data);
            response.PageSize = request.PageSize;
            response.PageNumber = request.PageNumber;
            response.TotalCount = totalCount;
            response.PageCount = pageCount;
            return response;
        }

        public async Task<ProductCreateResponse> Handle(ProductCreateRequest request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<ProductEntity>(request);

            await _context.Products.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<ProductCreateResponse>(entity);
        }

        public async Task<ProductEditResponse> Handle(ProductEditRequest request, CancellationToken cancellationToken)
        {
            var data = await _context.Products.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            if (data == null)
            {
                throw new ResourceNotFoundException();
            }

            data.Name = request.Name;
            data.StockCount = request.StockCount;

            await _context.SaveChangesAsync(cancellationToken);

            var message = _mapper.Map<ProductUpdatedEvent>(data);
            await _messageBus.PublishAsync(message);

            return _mapper.Map<ProductEditResponse>(data);
        }
    }
}