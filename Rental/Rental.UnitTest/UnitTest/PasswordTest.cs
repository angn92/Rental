using Moq;
using NUnit.Framework;
using Rental.Core.Enum;
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
        string password = String.Empty;
        string salt = String.Empty;
        string hashPassword = String.Empty;
        string passwordCode = String.Empty;

        [SetUp]
        public void InitializeData()
        {
            salt = "uFQSGu0/fAwkjs570aYMeg==";
            hashPassword = "$2a$08$nlcBlN7KSmfF3wz/jVcD.OzklD5qYxeaYpj/dSZh4n7v6YQAAekNG";
            passwordCode = "123456";
            password = "password123";
        } 

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
            var oldPassword = PasswordTestHelper.CreatePassword(_context, hashPassword, salt, customer, passwordCode, x =>
            {
                x.ActivatePassword();
            });

            // Act
            await passwordHelper.RemoveOldPassword(customer.Username);

            // Assert
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
            var oldPassword = PasswordTestHelper.CreatePassword(_context, hashPassword, salt, customer1, passwordCode, x =>
            {
                x.ActivatePassword();
            });

            var customer2 = CustomerTestHelper.CreateCustomer(_context, "Adam", "Nowak", "adam_n","adam@email.com", "746577477");

            // Act
            await passwordHelper.RemoveOldPassword(customer2.Username);

            // Assert
            var password1 = PasswordTestHelper.FindPasswordById(_context, oldPassword.PasswordId);
            Assert.IsNotNull(password1);

            var password2 = PasswordTestHelper.FindPasswordByUsername(_context, customer2.Username);
            Assert.IsNull(password2);
        }

        [Test]
        public async Task ShouldBeAbleSetPasswordForCustomer()
        {
            // Arrange
            var encryptMock = new Mock<IEncrypt>();
            encryptMock.Setup(x => x.GenerateSalt()).Returns(salt);
            encryptMock.Setup(x => x.GenerateHash(password, salt)).Returns(hashPassword);

            var passwordHelper = new PasswordHelper(_context, encryptMock.Object);
            var customer = CustomerTestHelper.CreateCustomer(_context, firstName, lastName, username, email, phone);

            // Act
            await passwordHelper.SetPassword(password, customer, passwordCode);

            // Assert
            var generatedPassword = PasswordTestHelper.FindPasswordByUsername(_context, customer.Username);
            Assert.IsNotNull(generatedPassword);
            Assert.IsNotEmpty(generatedPassword.Hash);
            Assert.IsNotEmpty(generatedPassword.Salt);
        }

        [Test]
        public void ShouldBeAbleGenerateHashForPassword()
        {
            // Arrange
            var encryptService = new EncryptService();

            // Act
            var hash = encryptService.GenerateHash(password, salt);

            // Assert
            Assert.IsNotNull(hash);
        }

        [Test]
        public void ShouldNotBeAbleGenerateHash_AtLeastOneArgumentIsNull_Password()
        {
            // Arrange
            var encryptService = new EncryptService();

            // Act
            var exception = Assert.Throws<ArgumentException>(() => encryptService.GenerateHash(null, salt));

            // Assert
            Assert.AreEqual("Can not generate hash from empty argument (Parameter 'password')", exception.Message);
        }

        [Test]
        public void ShouldNotBeAbleGenerateHash_AtLeastOneArgumentIsNull_Salt()
        {
            // Arrange
            var encryptService = new EncryptService();

            // Act
            var exception = Assert.Throws<ArgumentException>(() => encryptService.GenerateHash(password, null));

            // Assert
            Assert.AreEqual("Can not generate hash from empty argument (Parameter 'salt')", exception.Message);
        }

        [Test]
        public async Task ShouldBeAbleGetActivePassword()
        {
            // Arrange
            var encryptMock = new Mock<IEncrypt>();
            var passwordHelper = new PasswordHelper(_context, encryptMock.Object);
            var customer = CustomerTestHelper.CreateCustomer(_context, firstName, lastName, username, email, phone);
            var oldPassword = PasswordTestHelper.CreatePassword(_context, hashPassword, salt, customer, passwordCode, x =>
            {
                x.ActivatePassword();
            });

            // Act
            var activePassword = await passwordHelper.GetActivePassword(customer);

            // Assert
            Assert.AreEqual(PasswordStatus.Active.ToString(), activePassword.Status.ToString());
            Assert.IsNotNull(activePassword);
        }
    }
}
