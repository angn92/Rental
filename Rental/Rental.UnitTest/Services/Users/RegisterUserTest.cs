using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using Rental.Core.Domain;
using Rental.Core.Repository;
using Rental.Infrastructure.EF;
using Rental.Infrastructure.Exceptions;
using Rental.Infrastructure.Services.UserService;
using System;
using System.Threading.Tasks;

namespace Rental.UnitTest.Services.Users
{
    [TestFixture]
    public class RegisterUserTest
    {
        [Test]
        public async Task should_be_able_invoke_add_async_by_register_async()
        {
            
            var mockUserRepository = new Mock<IUserRepository>();

            var userService = new UserService(mockUserRepository.Object);

            await userService.RegisterAsync("user", "userLastName", "fakeuser", "user@example.com", "123456789");

            mockUserRepository.Verify(x => x.AddAsync(It.IsAny<User>()), Times.Once);
        }

        [Test]
        public void should_not_be_able_register_user_when_user_with_given_name_already_exists()
        {
            var mockUserRepository = new Mock<IUserRepository>();

            var userService = new UserService(mockUserRepository.Object);
            var user = new User("test1", "test2", "username", "email@example.com", "13456788");

            mockUserRepository.Setup(x => x.GetAsync(It.IsAny<string>()))
                              .ReturnsAsync(() => user);

            var exception = Assert.ThrowsAsync<CoreException>(() => userService.RegisterAsync("user", "userLastName", "fakeuser", "user@example.com", "123456789"));
            Assert.That(exception.Code, Is.EqualTo(ErrorCode.UsernameExist));
        }
    }
}
