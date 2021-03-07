using Moq;
using NUnit.Framework;
using Rental.Core.Domain;
using Rental.Core.Repository;
using Rental.Infrastructure.Exceptions;
using Rental.Infrastructure.Services.UserService;
using System.Threading.Tasks;
using FluentAssertions;
using Rental.Core.Enum;
using Rental.Infrastructure.Helpers;

namespace Rental.UnitTest.Services.Users
{
    [TestFixture]
    public class RegisterUserTest
    {

        [Test]
        public async Task should_be_able_invoke_add_async_by_register_async()
        {
            //Arrange
            var mockUserRepository = new Mock<IUserRepository>();
            var mockEmailValidator = new Mock<IEmailValidator>();
            
            var userService = new UserService(mockUserRepository.Object, mockEmailValidator.Object);

            //Act
            await userService.RegisterAsync("user", "userLastName", "fakeuser", "user@example.com", "123456789");

            //Assert
            mockUserRepository.Verify(x => x.AddAsync(It.IsAny<User>()), Times.Once);
        }

        [Test]
        public void should_not_be_able_register_user_when_user_with_given_name_already_exists()
        {
            //Arrange
            var mockUserRepository = new Mock<IUserRepository>();
            var mockEmailValidator = new Mock<IEmailValidator>();

            var user = new User("Adam", "Nowak", "adam123", "adam@example.com", "123456789");

            mockUserRepository.Setup(x => x.GetAsync(user.Username))
                              .ReturnsAsync(() => user);

            var userService = new UserService(mockUserRepository.Object, mockEmailValidator.Object);
            
            //Act
            var exception = Assert.ThrowsAsync<CoreException>(() => userService.RegisterAsync("Adam", "Nowak", "adam123", "adam@example.com", "123456789"));

            //Assert
            Assert.That(exception.Code, Is.EqualTo(ErrorCode.UsernameExist));
        }

        [Test]
        public async Task should_be_able_get_user_details()
        {
            //Arrange
            var mockUserRepository = new Mock<IUserRepository>();
            var mockEmailValidator = new Mock<IEmailValidator>();

            var userService = new UserService(mockUserRepository.Object, mockEmailValidator.Object);

            var user = new User("Adam", "Nowak", "adam123", "adam@example.com", "123456789");

            mockUserRepository.Setup(x => x.GetAsync(It.IsAny<string>()))
                              .ReturnsAsync(() => user);

            //Act
            var result = await userService.GetUserAsync("adam123");

            //Assert
            result.FullName.Should().Be(user.FirstName + " " + user.LastName);
            result.Status.Should().Be(AccountStatus.Active);
        }

        [Test]
        public void should_throw_exception_user_not_exist_when_given_username_not_exist_in_database()
        {
            //Arrange
            var mockUserRepository = new Mock<IUserRepository>();
            var mockEmailValidator = new Mock<IEmailValidator>();

            var userService = new UserService(mockUserRepository.Object, mockEmailValidator.Object);

            //Act
            var exception = Assert.ThrowsAsync<CoreException>(() => userService.GetUserAsync("adam123"));

            //Assert
            Assert.That(exception.Code, Is.EqualTo(ErrorCode.UserNotExist));
        }
    }
}
