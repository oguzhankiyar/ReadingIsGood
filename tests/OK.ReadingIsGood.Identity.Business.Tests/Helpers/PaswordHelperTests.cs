using System;
using OK.ReadingIsGood.Identity.Business.Helpers;
using Xunit;

namespace OK.ReadingIsGood.Identity.Business.Tests.Helpers
{
    public class PaswordHelperTests
    {
        [Theory]
        [InlineData("Pass*123")]
        [InlineData("3%AsXd")]
        [InlineData("12-sa23")]
        [InlineData("28051993")]
        public void PasswordHelper_ShouldWorkCorrectly(string password)
        {
            // Arrange
            var helper = new PasswordHelper();

            // Act
            var hash = helper.Hash(password);
            var verify = helper.Verify(password, hash);

            // Assert
            Assert.NotNull(hash);
            Assert.True(verify);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Hash_ShouldThrow_WhenHashIsInvalid(string password)
        {
            // Arrange
            var helper = new PasswordHelper();

            // Assert
            Assert.Throws<ArgumentException>(() =>
            {
                // Act
                _ = helper.Hash(password);
            });
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("asdas")]
        [InlineData("asdasada.213")]
        public void Verify_ShouldThrow_WhenHashIsInvalid(string hash)
        {
            // Arrange
            var helper = new PasswordHelper();

            // Assert
            Assert.Throws<ArgumentException>(() =>
            {
                // Act
                _ = helper.Verify("some_pass", hash);
            });
        }
    }
}