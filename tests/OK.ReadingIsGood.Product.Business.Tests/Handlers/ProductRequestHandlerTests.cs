using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using OK.ReadingIsGood.Product.Business.Handlers;
using OK.ReadingIsGood.Product.Contracts.Models;
using OK.ReadingIsGood.Product.Contracts.Requests;
using OK.ReadingIsGood.Product.Contracts.Responses;
using OK.ReadingIsGood.Product.Persistence.Contexts;
using OK.ReadingIsGood.Product.Persistence.Entities;
using OK.ReadingIsGood.Shared.Core.Events.Product;
using OK.ReadingIsGood.Shared.MessageBus.Abstractions;
using Xunit;

namespace OK.ReadingIsGood.Product.Business.Tests.Handlers
{
    public class ProductRequestHandlerTests
    {
        [Fact]
        public async Task ProductListRequest_ShouldHandleCorrectly()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ProductDataContext>()
                .UseInMemoryDatabase(databaseName: "RIG_Product")
                .Options;
            var context = new ProductDataContext(null, options);
            await context.Products.AddAsync(new ProductEntity
            {
                Name = "Book 1",
                StockCount = 100
            });
            await context.SaveChangesAsync();

            var messageBusMock = new Mock<IMessageBus>();
            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(x => x.Map<ProductListResponse>(It.IsAny<List<ProductEntity>>()))
                .Returns((List<ProductEntity> data) => new ProductListResponse
                {
                    Data = data.Select(x => new ProductModel
                    {
                        Id = x.Id,
                        Name = x.Name,
                        StockCount = x.StockCount
                    }).ToArray()
                });

            var handler = new ProductRequestHandler(
                context,
                messageBusMock.Object,
                mapperMock.Object);

            // Act
            var response = await handler.Handle(new ProductListRequest
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
            Assert.Equal("Book 1", response.Data.First().Name);
            Assert.Equal(100, response.Data.First().StockCount);
        }

        [Fact]
        public async Task ProductCreateRequest_ShouldHandleCorrectly()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ProductDataContext>()
                .UseInMemoryDatabase(databaseName: "RIG_Product")
                .Options;
            var context = new ProductDataContext(null, options);

            var messageBusMock = new Mock<IMessageBus>();
            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(x => x.Map<ProductEntity>(It.IsAny<ProductCreateRequest>()))
                .Returns((ProductCreateRequest request) => new ProductEntity
                {
                    Name = request.Name,
                    StockCount = request.StockCount
                });
            mapperMock
                .Setup(x => x.Map<ProductCreateResponse>(It.IsAny<ProductEntity>()))
                .Returns((ProductEntity data) => new ProductCreateResponse
                {
                    Data = new ProductModel
                    {
                        Id = data.Id,
                        Name = data.Name,
                        StockCount = data.StockCount
                    }
                });

            var handler = new ProductRequestHandler(
                context,
                messageBusMock.Object,
                mapperMock.Object);

            // Act
            var response = await handler.Handle(new ProductCreateRequest
            {
                Name = "Book 1",
                StockCount = 100
            }, CancellationToken.None);

            // Assert
            Assert.NotNull(response);
            Assert.True(response.Status);
            Assert.Null(response.Errors);
            Assert.NotNull(response.Data);
            Assert.Equal("Book 1", response.Data.Name);
            Assert.Equal(100, response.Data.StockCount);
        }

        [Fact]
        public async Task ProductEditRequest_ShouldHandleCorrectly()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ProductDataContext>()
                .UseInMemoryDatabase(databaseName: "RIG_Product")
                .Options;
            var context = new ProductDataContext(null, options);
            await context.Products.AddAsync(new ProductEntity
            {
                Name = "Book 1",
                StockCount = 100
            });
            await context.SaveChangesAsync();

            var messageBusMock = new Mock<IMessageBus>();
            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(x => x.Map<ProductEntity>(It.IsAny<ProductEditRequest>()))
                .Returns((ProductEditRequest request) => new ProductEntity
                {
                    Id = request.Id,
                    Name = request.Name,
                    StockCount = request.StockCount
                });
            mapperMock
                .Setup(x => x.Map<ProductEditResponse>(It.IsAny<ProductEntity>()))
                .Returns((ProductEntity data) => new ProductEditResponse
                {
                    Data = new ProductModel
                    {
                        Id = data.Id,
                        Name = data.Name,
                        StockCount = data.StockCount
                    }
                });

            var handler = new ProductRequestHandler(
                context,
                messageBusMock.Object,
                mapperMock.Object);

            // Act
            var response = await handler.Handle(new ProductEditRequest
            {
                Id = 1,
                Name = "Book 1",
                StockCount = 100
            }, CancellationToken.None);

            // Assert
            Assert.NotNull(response);
            Assert.True(response.Status);
            Assert.Null(response.Errors);
            Assert.NotNull(response.Data);
            Assert.Equal("Book 1", response.Data.Name);
            Assert.Equal(100, response.Data.StockCount);

            messageBusMock
                .Verify(x =>
                    x.PublishAsync(It.IsAny<ProductUpdatedEvent>(), It.IsAny<CancellationToken>()),
                    Times.Once);
        }
    }
}