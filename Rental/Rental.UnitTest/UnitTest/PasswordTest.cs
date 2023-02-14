using FluentAssertions;
using Moq;
using Npgsql.Internal.TypeHandlers;
using NUnit.Framework;
using Rental.Core.Enum;
using Rental.Infrastructure.Exceptions;
using Rental.Infrastructure.Helpers;
using Rental.Infrastructure.Services.EncryptService;
using Rental.Test.Helpers;
using System;
using System.Threading.Tasks;

namespace Rental.Test.UnitTest
{
    [TestFixture]
    public class PasswordTest : TestBase
    {
        private string password, salt, hashPassword, passwordCode = String.Empty;
        private Mock<IEncrypt> encryptMock;
        private PasswordHelper _passwordHelper;

        [SetUp]
        public void InitializeData()
        {
            encryptMock = new Mock<IEncrypt>();
            _passwordHelper = new PasswordHelper(_context, encryptMock.Object);

            salt = "uFQSGu0/fAwkjs570aYMeg==";
            hashPassword = "$2a$08$nlcBlN7KSmfF3wz/jVcD.OzklD5qYxeaYpj/dSZh4n7v6YQAAekNG";
            passwordCode = "123456";
            password = "password123";
        } 

        [Test]
        public void ShouldBeAbleGenerteActivationCode()
        {
            var activationCode = _passwordHelper.GenerateActivationCode().ToString();

            activationCode.Length.Should().Be(6);
            activationCode.Should().NotBeNull();
        }

        [Test]
        public void ShouldReturnPasswordToAuthorizeForGivenCustomer()
        {
            //ARRANGE
            var customer = CustomerTestHelper.CreateCustomer(_context, firstName, lastName, username, email, x =>
            {
                x.Status = AccountStatus.Active;
            });

            var password = PasswordTestHelper.CreatePassword(_context, "hashPassword123", "random_salt", customer, null, x =>
            {
                x.Status = PasswordStatus.NotActive;
            });

            //ACT
            var passwordToAuthorize = _passwordHelper.FindPasswordToAuthorize(username).Result;

            passwordToAuthorize.Should().NotBeNull();
            passwordToAuthorize.Status.Should().Be(PasswordStatus.NotActive);
        }

        [TestCase(PasswordStatus.Active)]
        [TestCase(PasswordStatus.Blocked)]
        public void ShouldReturnNull_PasswordToAuthorize_DoesNotExist(PasswordStatus passwordStatus)
        {
            //ARRANGE
            var customer = CustomerTestHelper.CreateCustomer(_context, firstName, lastName, username, email, x =>
            {
                x.Status = AccountStatus.Active;
            });

            PasswordTestHelper.CreatePassword(_context, "hashPassword123", "random_salt", customer, null, x =>
            {
                x.Status = passwordStatus;
            });

            //ACT
            var passwordToAuthorize = _passwordHelper.FindPasswordToAuthorize(username).Result;

            //ASSERT
            passwordToAuthorize.Should().BeNull();
        }

        [Test]
        public void ShouldBeAbleReturnActivePasswordForCustomer()
        {
            //ARRANGE
            var customer = CustomerTestHelper.CreateCustomer(_context, firstName, lastName, username, email, x =>
            {
                x.Status = AccountStatus.Active;
            });

            PasswordTestHelper.CreatePassword(_context, "hashPassword123", "random_salt", customer, null, x =>
            {
                x.Status = PasswordStatus.Active;
            });

            //ACT
            var password = _passwordHelper.GetActivePassword(customer).Result;

            //ASSERT
            password.Should().NotBeNull();
            password.Status.Should().Be(PasswordStatus.Active);
            password.Customer.Username.Should().Be(username);
        }

        [Test]
        public void ShouldNotBeAbleReturnPassword_CustomerHasNotActivePassword()
        {
            //ARRANGE
            var customer = CustomerTestHelper.CreateCustomer(_context, firstName, lastName, username, email, x =>
            {
                x.Status = AccountStatus.Active;
            });

            PasswordTestHelper.CreatePassword(_context, "hashPassword123", "random_salt", customer, null, x =>
            {
                x.Status = PasswordStatus.NotActive;
            });

            //ACT
            var exception = Assert.ThrowsAsync<CoreException>(() => _passwordHelper.GetActivePassword(customer));

            //ASSERT
            exception.Code.Should().Be(ErrorCode.PasswordNotExist);
        }

        [Test]
        public async Task ShoudBeAbleRemoveOldCustomerPassword()
        {
            //ARRANGE
            var customer = CustomerTestHelper.CreateCustomer(_context, firstName, lastName, username, email);
            var oldPassword = PasswordTestHelper.CreatePassword(_context, hashPassword, salt, customer, passwordCode, x =>
            {
                x.Status = PasswordStatus.Active;
            });

            //ACT
            await _passwordHelper.RemoveOldPassword(customer.Username);

            //ASSERT
            var password = PasswordTestHelper.FindPasswordById(_context, oldPassword.PasswordId);
            Assert.IsNull(password);
        }

        [Test]
        public async Task ShoudNotBeAbleRemoveOldPassword_WrongCustomer()
        {
            //ARRANGE
            var customer = CustomerTestHelper.CreateCustomer(_context, firstName, lastName, username, email);
            PasswordTestHelper.CreatePassword(_context, hashPassword, salt, customer, passwordCode, x =>
            {
                x.Status = PasswordStatus.Active;
            });

            //ACT
            await _passwordHelper.RemoveOldPassword("incorrectUsername");

            //ASSERT
            var password = PasswordTestHelper.FindPasswordByUsername(_context, customer.Username);
            password.Should().NotBeNull();
        }

        [Test]
        public async Task ShouldBeAbleSetPasswordForCustomer()
        {
            //ARRANGE
            var byteArray = new byte[byte.MaxValue];
            encryptMock.Setup(x => x.HashPasword(It.IsAny<string>(), out byteArray)).Returns(hashPassword);
            
            var customer = CustomerTestHelper.CreateCustomer(_context, firstName, lastName, username, email);

            //ACT
            await _passwordHelper.SetPassword(password, customer, null);

            //ASSERT
            var generatedPassword = PasswordTestHelper.FindPasswordByUsername(_context, customer.Username);
            generatedPassword.Should().NotBeNull();
            generatedPassword.Hash.Should().NotBeEmpty();
            generatedPassword.Salt.Should().NotBeEmpty();
        }
    }
}
