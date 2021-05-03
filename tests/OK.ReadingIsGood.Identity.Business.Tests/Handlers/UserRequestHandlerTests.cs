using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using OK.ReadingIsGood.Identity.Business.Config;
using OK.ReadingIsGood.Identity.Business.Handlers;
using OK.ReadingIsGood.Identity.Business.Helpers;
using OK.ReadingIsGood.Identity.Contracts.Models;
using OK.ReadingIsGood.Identity.Contracts.Requests;
using OK.ReadingIsGood.Identity.Contracts.Responses;
using OK.ReadingIsGood.Identity.Persistence.Contexts;
using OK.ReadingIsGood.Identity.Persistence.Entities;
using OK.ReadingIsGood.Shared.Core.Events.User;
using OK.ReadingIsGood.Shared.MessageBus.Abstractions;
using Xunit;

namespace OK.ReadingIsGood.Identity.Business.Tests.Handlers
{
    public class UserRequestHandlerTests
    {
        [Fact]
        public async Task UserListRequest_ShouldHandleCorrectly()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<IdentityDataContext>()
                .UseInMemoryDatabase(databaseName: "RIG_Identity")
                .Options;
            var context = new IdentityDataContext(null, options);
            await context.Users.AddAsync(new UserEntity
            {
                FullName = "Oğuzhan Kiyar",
                Email = "oguzhan@kiyar.io",
                Password = "hashed_pass"
            });
            await context.SaveChangesAsync();

            var passwordHelperMock = new Mock<IPasswordHelper>();
            passwordHelperMock
                .Setup(x => x.Verify(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(true);

            var messageBusMock = new Mock<IMessageBus>();
            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(x => x.Map<UserListResponse>(It.IsAny<List<UserEntity>>()))
                .Returns((List<UserEntity> data) => new UserListResponse
                {
                    Data = data.Select(x => new UserModel
                    {
                        Id = x.Id,
                        Email = x.Email,
                        FullName = x.FullName
                    }).ToArray()
                });

            var handler = new UserRequestHandler(
                context,
                passwordHelperMock.Object,
                messageBusMock.Object,
                mapperMock.Object);

            // Act
            var response = await handler.Handle(new UserListRequest
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
            Assert.Equal("Oğuzhan Kiyar", response.Data.First().FullName);
            Assert.Equal("oguzhan@kiyar.io", response.Data.First().Email);
        }

        [Fact]
        public async Task UserCreateRequest_ShouldHandleCorrectly()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<IdentityDataContext>()
                .UseInMemoryDatabase(databaseName: "RIG_Identity")
                .Options;
            var context = new IdentityDataContext(null, options);

            var passwordHelperMock = new Mock<IPasswordHelper>();
            passwordHelperMock
                .Setup(x => x.Verify(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(true);

            var messageBusMock = new Mock<IMessageBus>();
            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(x => x.Map<UserEntity>(It.IsAny<UserCreateRequest>()))
                .Returns((UserCreateRequest request) => new UserEntity
                {
                    Email = request.Email,
                    FullName = request.FullName,
                    Password = request.Password
                });
            mapperMock
                .Setup(x => x.Map<UserCreateResponse>(It.IsAny<UserEntity>()))
                .Returns((UserEntity data) => new UserCreateResponse
                {
                    Data = new UserModel
                    {
                        Id = data.Id,
                        Email = data.Email,
                        FullName = data.FullName
                    }
                });

            var handler = new UserRequestHandler(
                context,
                passwordHelperMock.Object,
                messageBusMock.Object,
                mapperMock.Object);

            // Act
            var response = await handler.Handle(new UserCreateRequest
            {
                FullName = "Oğuzhan Kiyar",
                Email = "oguzhan@kiyar.io",
                Password = "123456",
                ConfirmPassword = "123456"
            }, CancellationToken.None);

            // Assert
            Assert.NotNull(response);
            Assert.True(response.Status);
            Assert.Null(response.Errors);
            Assert.NotNull(response.Data);
            Assert.Equal("Oğuzhan Kiyar", response.Data.FullName);
            Assert.Equal("oguzhan@kiyar.io", response.Data.Email);

            messageBusMock
                .Verify(x =>
                    x.PublishAsync(It.IsAny<UserCreatedEvent>(), It.IsAny<CancellationToken>()),
                    Times.Once);
        }
    }
}