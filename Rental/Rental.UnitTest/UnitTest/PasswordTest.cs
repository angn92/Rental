using FluentAssertions;
using Moq;
using NUnit.Framework;
using Rental.Infrastructure.Exceptions;
using Rental.Infrastructure.Helpers;
using Rental.Infrastructure.Services.EncryptService;
using Rental.Test.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rental.Test.UnitTest
{
    [TestFixture]
    public class PasswordTest : TestBase
    {
        [Test]
        public void ShouldBeAbleGenerteActivationCode()
        {
            // Arrange
            var encryptMock = new Mock<IEncrypt>();
            var passwordHelper = new PasswordHelper(_context, encryptMock.Object);

            // Act
            var activationCode = passwordHelper.GenerateActivationCode().ToString();

            // Assert
            Assert.NotNull(activationCode);
            Assert.AreEqual(6, activationCode.Length);
        }

        [Test]
        public async Task ShoudBeAbleRemoveOldCustomerPassword()
        {
            // Arrange
            var encryptMock = new Mock<IEncrypt>();
            var passwordHelper = new PasswordHelper(_context, encryptMock.Object);
            var customer = CustomerTestHelper.CreateCustomer(_context, firstName, lastName, username, email, phone);
            var oldPassword = PasswordTestHelper.CreatePassword(_context, "hash", "salt", customer, "123456", x =>
            {
                x.ActivatePassword();
            });

            // Act
            await passwordHelper.RemoveOldPassword(customer.Username);

            // Arrange
            var password = PasswordTestHelper.FindPasswordById(_context, oldPassword.PasswordId);
            Assert.IsNull(password);
        }

        [Test]
        public async Task ShoudNotBeAbleRemoveOldPassword_PasswordNotExist()
        {
            // Arrange
            var encryptMock = new Mock<IEncrypt>();
            var passwordHelper = new PasswordHelper(_context, encryptMock.Object);
            var customer1 = CustomerTestHelper.CreateCustomer(_context, firstName, lastName, username, email, phone);
            var oldPassword = PasswordTestHelper.CreatePassword(_context, "hash", "salt", customer1, "123456", x =>
            {
                x.ActivatePassword();
            });

            var customer2 = CustomerTestHelper.CreateCustomer(_context, "Adam", "Nowak", "adam_n","adam@email.com", "746577477");

            // Act
            await passwordHelper.RemoveOldPassword(customer2.Username);

            // Arrange
            var password1 = PasswordTestHelper.FindPasswordById(_context, oldPassword.PasswordId);
            Assert.IsNotNull(password1);

            var password2 = PasswordTestHelper.FindPasswordByUsername(_context, customer2.Username);
            Assert.IsNull(password2);

            
        }
    }
}
