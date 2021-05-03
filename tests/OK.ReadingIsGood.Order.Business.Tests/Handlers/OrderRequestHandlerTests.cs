using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using OK.ReadingIsGood.Order.Business.Handlers;
using OK.ReadingIsGood.Order.Contracts.Models;
using OK.ReadingIsGood.Order.Contracts.Requests;
using OK.ReadingIsGood.Order.Contracts.Responses;
using OK.ReadingIsGood.Order.Persistence.Contexts;
using OK.ReadingIsGood.Order.Persistence.Entities;
using OK.ReadingIsGood.Shared.Core.Events.Order;
using OK.ReadingIsGood.Shared.MessageBus.Abstractions;
using Xunit;

namespace OK.ReadingIsGood.Order.Business.Tests.Handlers
{
    public class OrderRequestHandlerTests
    {
        [Fact]
        public async Task OrderListRequest_ShouldHandleCorrectly()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<OrderDataContext>()
                .UseInMemoryDatabase(databaseName: "RIG_Order")
                .Options;
            var context = new OrderDataContext(null, options);
            await context.Orders.AddAsync(new OrderEntity
            {
                Id = 1,
                UserId = 1,
                StatusId = 1,
                Items = new List<OrderItemEntity>
                {
                    new OrderItemEntity { ProductId = 1, Quantity = 2 }
                }
            });
            await context.SaveChangesAsync();

            var messageBusMock = new Mock<IMessageBus>();
            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(x => x.Map<OrderListResponse>(It.IsAny<List<OrderEntity>>()))
                .Returns((List<OrderEntity> data) => new OrderListResponse
                {
                    Data = data.Select(x => new OrderModel
                    {
                        Id = x.Id,
                        UserId = x.UserId,
                        StatusId = x.StatusId,
                        Items = x.Items.Select(y => new OrderItemModel
                        {
                            ProductId = y.ProductId,
                            Quantity = y.Quantity
                        }).ToList()
                    }).ToArray()
                });

            var handler = new OrderRequestHandler(
                context,
                messageBusMock.Object,
                mapperMock.Object);

            // Act
            var response = await handler.Handle(new OrderListRequest
            {
                PageNumber = 1,
                PageSize = 10
            }, CancellationToken.None);

            // Assert
            Assert.NotNull(response);
            Assert.True(response.Status);
            Assert.Null(response.Errors);
            Assert.NotNull(response.Data);
            Assert.NotEmpty(response.Data);
            Assert.Equal(1, response.Data.First().UserId);
            Assert.Equal(1, response.Data.First().StatusId);
            Assert.NotEmpty(response.Data.First().Items);
            Assert.Equal(1, response.Data.First().Items.First().ProductId);
            Assert.Equal(2, response.Data.First().Items.First().Quantity);
        }

        [Fact]
        public async Task OrderDetailRequest_ShouldHandleCorrectly()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<OrderDataContext>()
                .UseInMemoryDatabase(databaseName: "RIG_Order")
                .Options;
            var context = new OrderDataContext(null, options);
            await context.Orders.AddAsync(new OrderEntity
            {
                Id = 1,
                UserId = 1,
                StatusId = 1,
                Items = new List<OrderItemEntity>
                {
                    new OrderItemEntity { ProductId = 1, Quantity = 2 }
                }
            });
            await context.SaveChangesAsync();

            var messageBusMock = new Mock<IMessageBus>();
            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(x => x.Map<OrderDetailResponse>(It.IsAny<OrderEntity>()))
                .Returns((OrderEntity data) => new OrderDetailResponse
                {
                    Data = new OrderModel
                    {
                        Id = data.Id,
                        UserId = data.UserId,
                        StatusId = data.StatusId,
                        Items = data.Items.Select(x => new OrderItemModel
                        {
                            ProductId = x.ProductId,
                            Quantity = x.Quantity
                        }).ToList()
                    }
                });

            var handler = new OrderRequestHandler(
                context,
                messageBusMock.Object,
                mapperMock.Object);

            // Act
            var response = await handler.Handle(new OrderDetailRequest
            {
                Id = 1
            }, CancellationToken.None);

            // Assert
            Assert.NotNull(response);
            Assert.True(response.Status);
            Assert.Null(response.Errors);
            Assert.NotNull(response.Data);
            Assert.Equal(1, response.Data.UserId);
            Assert.Equal(1, response.Data.StatusId);
            Assert.NotEmpty(response.Data.Items);
            Assert.Equal(1, response.Data.Items.First().ProductId);
            Assert.Equal(2, response.Data.Items.First().Quantity);
        }

        [Fact]
        public async Task OrderCreateRequest_ShouldHandleCorrectly()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<OrderDataContext>()
                .UseInMemoryDatabase(databaseName: "RIG_Order")
                .Options;
            var context = new OrderDataContext(null, options);

            var messageBusMock = new Mock<IMessageBus>();
            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(x => x.Map<OrderEntity>(It.IsAny<OrderCreateRequest>()))
                .Returns((OrderCreateRequest request) => new OrderEntity
                {
                    UserId = 1,
                    StatusId = 1,
                    Items = request.Items.Select(x => new OrderItemEntity
                    {
                        ProductId = x.ProductId,
                        Quantity = x.Quantity
                    }).ToList()
                });
            mapperMock
                .Setup(x => x.Map<OrderCreateResponse>(It.IsAny<OrderEntity>()))
                .Returns((OrderEntity data) => new OrderCreateResponse
                {
                    Data = new OrderModel
                    {
                        Id = data.Id,
                        UserId = 1,
                        StatusId = 1,
                        Items = data.Items.Select(x => new OrderItemModel
                        {
                            ProductId = x.ProductId,
                            Quantity = x.Quantity
                        }).ToList()
                    }
                });

            var handler = new OrderRequestHandler(
                context,
                messageBusMock.Object,
                mapperMock.Object);

            // Act
            var response = await handler.Handle(new OrderCreateRequest
            {
                Items = new List<OrderItemModel>
                {
                    new OrderItemModel { ProductId = 1, Quantity = 2 }
                }
            }, CancellationToken.None);

            // Assert
            Assert.NotNull(response);
            Assert.True(response.Status);
            Assert.Null(response.Errors);
            Assert.NotNull(response.Data);
            Assert.NotEmpty(response.Data.Items);
            Assert.Equal(1, response.Data.Items.First().ProductId);
            Assert.Equal(2, response.Data.Items.First().Quantity);

            messageBusMock
                .Verify(x =>
                    x.PublishAsync(It.IsAny<OrderCreatedEvent>(), It.IsAny<CancellationToken>()),
                    Times.Once);
        }
    }
}