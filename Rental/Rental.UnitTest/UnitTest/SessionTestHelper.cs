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
    public class SessionTestHelper : TestBase
    {
        [Test]
        [TestCase(SessionState.Active)]
        [TestCase(SessionState.NotAuthorized)]
        [TestCase(SessionState.Expired)]
        public void ShouldVerifySessionStatus(SessionState sessionState)
        {
            // Arrange
            var sessioId = 123456;
            var customer = CreateCustomerTestHelper.CreateCustomer(_context, firstName, lastName, username, email, phone);
            var session = CreateSessionTestHelper.CreateSession(_context, sessioId, customer, sessionState);
            var sessionHelper = new SessionHelper();

            // Act
            var sessionStatusResult = sessionHelper.CheckSessionStatus(session);

            // Assert
            Assert.AreEqual(sessionState, sessionStatusResult);
        }

        [Test]
        public void ShouldBeAbleVerifySessionIsExpired()
        {
            // Arrange
            var sessioId = 123456;
            var customer = CreateCustomerTestHelper.CreateCustomer(_context, firstName, lastName, username, email, phone);
            var session = CreateSessionTestHelper.CreateSession(_context, sessioId, customer, SessionState.Active, x =>
            {
                x.GenerationDate = DateTime.UtcNow.AddMinutes(-20);
                x.LastAccessDate = DateTime.UtcNow.AddMinutes(-10);
            });

            var sessionHelper = new SessionHelper();

            // Act
            var sessionStatusResult = sessionHelper.SessionExpired(session);

            // Assert
            Assert.That(sessionStatusResult, Is.True);
        }

        [Test]
        public void ShouldBeAbleValidateGivenSession_WhenSessionNotExist()
        {
            // Arrange
            var sessioId = 123456;
            var customer = CreateCustomerTestHelper.CreateCustomer(_context, firstName, lastName, username, email, phone);
            var session = CreateSessionTestHelper.CreateSession(_context, sessioId, customer, SessionState.Active);

            var sessionHelper = new SessionHelper();

            // Act
            var exception = Assert.Throws<CoreException>(() => sessionHelper.ValidateSession(null));

            // Assert
            Assert.AreEqual(ErrorCode.SessionDoesNotExist, exception.Code);
        }

        [Test]
        public void ShouldReturnSessionNotAuthorized()
        {
            // Arrange
            var sessioId = 123456;
            var customer = CreateCustomerTestHelper.CreateCustomer(_context, firstName, lastName, username, email, phone);
            var session = CreateSessionTestHelper.CreateSession(_context, sessioId, customer, SessionState.NotAuthorized);

            var sessionHelper = new SessionHelper();

            // Act
            var exception = Assert.Throws<CoreException>(() => sessionHelper.ValidateSession(session));

            // Assert
            Assert.AreEqual(ErrorCode.SessionNotAuthorized, exception.Code);
        }

        [Test]
        public void ShouldReturnSessionExpired()
        {
            // Arrange
            var sessioId = 123456;
            var customer = CreateCustomerTestHelper.CreateCustomer(_context, firstName, lastName, username, email, phone);
            var session = CreateSessionTestHelper.CreateSession(_context, sessioId, customer, SessionState.Expired);

            var sessionHelper = new SessionHelper();

            // Act
            var exception = Assert.Throws<CoreException>(() => sessionHelper.ValidateSession(session));

            // Assert
            Assert.AreEqual(ErrorCode.SessionExpired, exception.Code);
        }

        [Test]
        public async Task ShouldBeAbleGetOnlyOneSession_WithGivenId()
        {
            // Arrange
            var sessioId = 123456;
            var customer = CreateCustomerTestHelper.CreateCustomer(_context, firstName, lastName, username, email, phone);
            CreateSessionTestHelper.CreateSession(_context, sessioId, customer, SessionState.Active);

            var sessionHelper = new SessionHelper();

            // Act
            var currentSession = await sessionHelper.GetSessionByIdAsync(_context, sessioId);

            // Assert
            Assert.That(currentSession, Is.Not.Null);
        }

        [Test]
        public async Task ShouldNotBeAbleGetSession_WrongGivenId()
        {
            // Arrange
            var sessioId = 123456;
            var wrongSessionId = 654321;
            var customer = CreateCustomerTestHelper.CreateCustomer(_context, firstName, lastName, username, email, phone);
            CreateSessionTestHelper.CreateSession(_context, sessioId, customer, SessionState.Active);

            var sessionHelper = new SessionHelper();

            // Act
            var currentSession = await sessionHelper.GetSessionByIdAsync(_context, wrongSessionId);

            // Assert
            Assert.That(currentSession, Is.Null);
        }
    }
}
