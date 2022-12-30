using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using Rental.Core.Enum;
using Rental.Infrastructure.Exceptions;
using Rental.Infrastructure.Helpers;
using Rental.Test.Helpers;
using System;
using System.Threading.Tasks;

namespace Rental.Test.UnitTest
{
    [TestFixture]
    public class CustomerTest : TestBase
    {
        [Test]
        public void ShouldBeAbleCheckThatGivenUsernameExist()
        {
            // Arrange
            var emailMock = new Mock<IEmailHelper>();

            CustomerTestHelper.CreateCustomer(_context, firstName, lastName, username, email, null);
            var customerHelper = new CustomerHelper(_context, emailMock.Object);

            // Act
            var exist = customerHelper.CheckIfExist(username);

            // Assert
            Assert.IsTrue(exist);
        }

        [Test]
        public void ShouldBReturnFalseWhenGivenCustomerNotExist()
        {
            // Arrange
            var emailMock = new Mock<IEmailHelper>();

            CustomerTestHelper.CreateCustomer(_context, firstName, lastName, username, email, null);
            var customerHelper = new CustomerHelper(_context, emailMock.Object);

            // Act
            var exist = customerHelper.CheckIfExist(username);

            // Assert
            Assert.IsFalse(exist);
        }

        [Test]
        public async Task ShouldBeAbleGetCustomer()
        {
            // Arrange
            var emailMock = new Mock<IEmailHelper>();

            CustomerTestHelper.CreateCustomer(_context, firstName, lastName, username, email, null);
            var customerHelper = new CustomerHelper(_context, emailMock.Object);

            // Act
            var user = await customerHelper.GetCustomerAsync(username);

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

            CustomerTestHelper.CreateCustomer(_context, firstName, lastName, username, email, null);
            var customerHelper = new CustomerHelper(_context, emailMock.Object);

            // Act
            var exception = Assert.ThrowsAsync<CoreException>(() => customerHelper.GetCustomerAsync("wrong_username"));

            // Assert
            Assert.AreEqual(ErrorCode.UserNotExist, exception.Code);
        }

        [Test]
        public async Task ShouldBeAbleRegisterUser()
        {
            // Arrange
            var emailMock = new Mock<IEmailHelper>();
            var customerHelper = new CustomerHelper(_context, emailMock.Object);

            // Act
            await customerHelper.RegisterAsync("Jan", "Kowalski", "kowal123", "kowal@email.com");

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
           
            var customerHelper = new CustomerHelper(_context, emailMock.Object);
            string FirstName = null;

            // Act
            var exception = Assert.ThrowsAsync<Exception>(() => customerHelper.RegisterAsync(FirstName, "Kowalski", "kowal123", 
                                                                    "kowal@email.com"));

            // Assert
            Assert.AreEqual($"Registration is failed. Value cannot be null. (Parameter '{nameof(FirstName)}')", exception.Message);
        }

        [Test]
        public void ShouldBeAbleValidateCustomerAccount_AndThrowException_AccountIsBlocked()
        {
            // Arrange
            var emailMock = new Mock<IEmailHelper>();

            var customer = CustomerTestHelper.CreateCustomer(_context, "Jan", "Kowalski", "janek00", "jan@email.com", x =>
            {
                x.Status = AccountStatus.Blocked;
            });

            var customerHelper = new CustomerHelper(_context, emailMock.Object);

            // Act
            var result = Assert.Throws<CoreException>(() => customerHelper.ValidateCustomerAccount(customer));

            // Assert
            Assert.AreEqual(ErrorCode.AccountBlocked, result.Code);
            Assert.AreEqual("Account is blocked.", result.Message);
        }

        [Test]
        public void ShouldBeAbleValidateCustomerAccount_AndThrowException_AccountIsNotActive()
        {
            // Arrange
            var emailMock = new Mock<IEmailHelper>();

            var customer = CustomerTestHelper.CreateCustomer(_context, "Jan", "Kowalski", "janek00", "jan@email.com", x =>
            {
                x.Status = AccountStatus.NotActive;
            });

            var customerHelper = new CustomerHelper(_context, emailMock.Object);

            // Act
            var result = Assert.Throws<CoreException>(() => customerHelper.ValidateCustomerAccount(customer));

            // Assert
            Assert.AreEqual(ErrorCode.AccountNotActive, result.Code);
            Assert.AreEqual("Account is not active.", result.Message);
        }
    }
}
