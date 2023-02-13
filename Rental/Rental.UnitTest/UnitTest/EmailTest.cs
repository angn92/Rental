using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using Rental.Core.Enum;
using Rental.Infrastructure.Configuration;
using Rental.Infrastructure.Exceptions;
using Rental.Infrastructure.Helpers;
using Rental.Test.Helpers;
using System;

namespace Rental.Test.UnitTest
{
    [TestFixture]
    public class EmailTest : TestBase
    {
        public Mock<IOptions<ConfigurationOptions>> optionsMock;
        public EmailHelper emailHelper;

        IOptions<ConfigurationOptions> someOptions = Options.Create<ConfigurationOptions>(new ConfigurationOptions()
        {
            EmailAddress = "andrzej@gmail.com"
        });

        [SetUp]
        public void SetUp()
        {
            optionsMock = new Mock<IOptions<ConfigurationOptions>>();
            emailHelper = new EmailHelper(_context, someOptions);
        }

        [Test]
        public void ShouldBeThrowException_EmailParameterIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => emailHelper.ValidateEmail(null));

            exception.Message.Should().Be("Value cannot be null. (Parameter 'email')");
        }

        [Test]
        public void ShouldThrowInvalidEmail()
        {
            var exception = Assert.Throws<CoreException>(() => emailHelper.ValidateEmail("wrongEmail"));

            exception.Code.Should().Be(ErrorCode.InvalidEmail);
        }

        [Test]
        public void ShouldThrowErrorEmailInUse()
        {
            var customer = CustomerTestHelper.CreateCustomer(_context, firstName, lastName, username, email, x =>
            {
                x.Status = AccountStatus.NotActive;
            });

            var exception = Assert.Throws<CoreException>(() => emailHelper.ValidateEmail(email));

            exception.Code.Should().Be(ErrorCode.EmailInUse);
        }

        [Test]
        public void ShouldBeAblePrepareEmail()
        {
            var emailConfig = emailHelper.PrepareEmail(email, SubjectMessage.RegistrationAccount, "Registration new customer account");

            emailConfig.Should().NotBeNull();
        }
    }
}
