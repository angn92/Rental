using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using Rental.Core.Enum;
using Rental.Infrastructure.Exceptions;
using Rental.Infrastructure.Helpers;
using Rental.Infrastructure.Services.CustomerService;
using Rental.Test.Helpers;
using System;
using System.Threading.Tasks;

namespace Rental.Test.UnitTest
{
    [TestFixture]
    public class CustomerServicesTest : TestBase
    {
        [Test]
        public async Task ShouldBeAbleCheckThatGivenUsernameExist()
        {
            // Arrange
            var emailMock = new Mock<IEmailHelper>();
            var passwordMock = new Mock<IPasswordHelper>();

            CustomerTestHelper.CreateCustomer(_context, firstName, lastName, username, email, phone, null);
            var userService = new CustomerService(_context, emailMock.Object, passwordMock.Object);

            // Act
            var exist = await userService.CheckIfExist(username);

            // Assert
            Assert.IsTrue(exist);
        }

        [Test]
        public async Task ShouldBReturnFalseWhenGivenCustomerNotExist()
        {
            // Arrange
            var emailMock = new Mock<IEmailHelper>();
            var passwordMock = new Mock<IPasswordHelper>();

            CustomerTestHelper.CreateCustomer(_context, firstName, lastName, username, email, phone, null);
            var userService = new CustomerService(_context, emailMock.Object, passwordMock.Object);

            // Act
            var exist = await userService.CheckIfExist("wrong_username");

            // Assert
            Assert.IsFalse(exist);
        }

        [Test]
        public async Task ShouldBeAbleGetCustomer()
        {
            // Arrange
            var emailMock = new Mock<IEmailHelper>();
            var passwordMock = new Mock<IPasswordHelper>();

            CustomerTestHelper.CreateCustomer(_context, firstName, lastName, username, email, null, null);
            var userService = new CustomerService(_context, emailMock.Object, passwordMock.Object);

            // Act
            var user = await userService.GetCustomerAsync(username);

            // Assert
            Assert.IsNotNull(user);
            Assert.AreEqual(username, user.Username);
            Assert.AreEqual(AccountStatus.Active.ToString(), user.Status.ToString());
            Assert.IsNull(user.Phone);
        }

        [Test]
        public async Task ShouldReturnErrorMessageWhenUserDoesNotExist()
        {
            // Arrange
            var emailMock = new Mock<IEmailHelper>();
            var passwordMock = new Mock<IPasswordHelper>();

            CustomerTestHelper.CreateCustomer(_context, firstName, lastName, username, email, phone, null);
            var userService = new CustomerService(_context, emailMock.Object, passwordMock.Object);

            // Act
            var exception = Assert.ThrowsAsync<CoreException>(() => userService.GetCustomerAsync("wrong_username"));

            // Assert
            Assert.AreEqual(ErrorCode.UserNotExist, exception.Code);
        }

        [Test]
        public async Task ShouldBeAbleRegisterUser()
        {
            // Arrange
            var emailMock = new Mock<IEmailHelper>();
            var passwordMock = new Mock<IPasswordHelper>();
            var userService = new CustomerService(_context, emailMock.Object, passwordMock.Object);

            // Act
            await userService.RegisterAsync("Jan", "Kowalski", "kowal123", "kowal@email.com", "123123123");

            var registeredUser = await _context.Customers.FirstOrDefaultAsync(x => x.Username == "kowal123");

            // Assert 
            Assert.NotNull(registeredUser);
            Assert.AreEqual("kowal123", registeredUser.Username);
        }

        [Test]
        public async Task ShouldNotBeAbleRegisterCustomer_InvalidInputData()
        {
            // Arrange
            var emailMock = new Mock<IEmailHelper>();
            var passwordMock = new Mock<IPasswordHelper>();
            var userService = new CustomerService(_context, emailMock.Object, passwordMock.Object);
            string FirstName = null;

            // Act
            var exception = Assert.ThrowsAsync<Exception>(() => userService.RegisterAsync(FirstName, "Kowalski", "kowal123", 
                                                                    "kowal@email.com", "123123123"));

            // Assert
            Assert.AreEqual($"Registration is failed. Value cannot be null. (Parameter '{nameof(FirstName)}')", exception.Message);
        }

        [Test]
        public void ShouldBeAbleValidateCustomerAccount_AndThrowException_AccountIsBlocked()
        {
            // Arrange
            var emailMock = new Mock<IEmailHelper>();
            var passwordMock = new Mock<IPasswordHelper>();

            var customer = CustomerTestHelper.CreateCustomer(_context, "Jan", "Kowalski", "janek00", "jan@email.com", null, x =>
            {
                x.Status = AccountStatus.Blocked;
            });

            var userService = new CustomerService(_context, emailMock.Object, passwordMock.Object);

            // Act
            var result = Assert.Throws<CoreException>(() => userService.ValidateCustomerAccount(customer));

            // Assert
            Assert.AreEqual(ErrorCode.AccountBlocked, result.Code);
            Assert.AreEqual("Account is blocked.", result.Message);
        }

        [Test]
        public void ShouldBeAbleValidateCustomerAccount_AndThrowException_AccountIsNotActive()
        {
            // Arrange
            var emailMock = new Mock<IEmailHelper>();
            var passwordMock = new Mock<IPasswordHelper>();

            var customer = CustomerTestHelper.CreateCustomer(_context, "Jan", "Kowalski", "janek00", "jan@email.com", null, x =>
            {
                x.Status = AccountStatus.NotActive;
            });

            var userService = new CustomerService(_context, emailMock.Object, passwordMock.Object);

            // Act
            var result = Assert.Throws<CoreException>(() => userService.ValidateCustomerAccount(customer));

            // Assert
            Assert.AreEqual(ErrorCode.AccountNotActive, result.Code);
            Assert.AreEqual("Account is not active.", result.Message);
        }
    }
}
