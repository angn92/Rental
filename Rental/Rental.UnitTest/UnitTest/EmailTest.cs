using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using Rental.Core.Enum;
using Rental.Infrastructure.Configuration;
using Rental.Infrastructure.EF;
using Rental.Infrastructure.Exceptions;
using Rental.Infrastructure.Helpers;
using Rental.Test.Helpers;
using System;

namespace Rental.Test.UnitTest
{
    [TestFixture]
    public class EmailTest : TestBase
    {
        [Test]
        public void ShouldBeThrowException_EmailParameterIsNull()
        {
            //Arrange
            var optionsMock = new Mock<IOptions<ConfigurationOptions>>();

            var email = new EmailHelper(_context, optionsMock.Object);

            // Act
            var exception = Assert.Throws<ArgumentNullException>(() => email.ValidateEmail(null));

            // Assert
            Assert.AreEqual("Value cannot be null. (Parameter 'email')", exception.Message);
        }

        [Test]
        public void ShouldThrowInvalidEmail()
        {
            //Arrange
            var optionsMock = new Mock<IOptions<ConfigurationOptions>>();

            var email = new EmailHelper(_context, optionsMock.Object);

            // Act
            var exception = Assert.Throws<CoreException>(() => email.ValidateEmail("wrongEmail"));

            // Assert
            Assert.AreEqual(ErrorCode.InvalidEmail, exception.Code);
        }

        [Test]
        public void ShouldThrowErrorEmailInUse()
        {
            //Arrange
            var optionsMock = new Mock<IOptions<ConfigurationOptions>>();
            var email = new EmailHelper(_context, optionsMock.Object);

            var customer = CustomerTestHelper.CreateCustomer(_context, "Jan", "Kowalski", "janek00", "jan@email.com", x =>
            {
                x.Status = AccountStatus.NotActive;
            });

            // Act
            var exception = Assert.Throws<CoreException>(() => email.ValidateEmail("jan@email.com"));

            // Assert
            Assert.AreEqual(ErrorCode.EmailInUse, exception.Code);
        }
    }
}
