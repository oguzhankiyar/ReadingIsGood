using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using OK.ReadingIsGood.Identity.Business.Config;
using OK.ReadingIsGood.Identity.Business.Handlers;
using OK.ReadingIsGood.Identity.Business.Helpers;
using OK.ReadingIsGood.Identity.Contracts.Requests;
using OK.ReadingIsGood.Identity.Persistence.Contexts;
using OK.ReadingIsGood.Identity.Persistence.Entities;
using Xunit;

namespace OK.ReadingIsGood.Identity.Business.Tests.Handlers
{
    public class AuthRequestHandlerTests
    {
        [Fact]
        public async Task AuthTokenRequest_ShouldHandleCorrectly()
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

            var config = new IdentityBusinessConfig
            {
                Audience = "kiyar.io",
                Issuer = "kiyar.io",
                SecurityKey = "SUPER_SECRET_KEY",
                ExpirationMinutes = 60
            };

            var handler = new AuthRequestHandler(
                context,
                passwordHelperMock.Object,
                config);

            // Act
            var response = await handler.Handle(new AuthTokenRequest
            {
                Email = "oguzhan@kiyar.io",
                Password = "123456"
            }, CancellationToken.None);

            // Assert
            Assert.NotNull(response);
            Assert.True(response.Status);
            Assert.Null(response.Errors);
            Assert.NotNull(response.Data);
            Assert.NotNull(response.Data.AccessToken);
        }
    }
}