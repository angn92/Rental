using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Rental.Core.Domain;
using Rental.Core.Enum;
using Rental.Infrastructure.Exceptions;
using Rental.Infrastructure.Helpers;
using Rental.Test.Helpers;
using System;
using System.Threading.Tasks;

namespace Rental.Test.UnitTest
{
    [TestFixture]
    public class SessionTest : TestBase
    {
        private Mock<ILogger<SessionHelper>> loggeMock;
        private SessionHelper sessionHelper;

        [SetUp]
        public void SetUp()
        {
            loggeMock = new Mock<ILogger<SessionHelper>>();
            sessionHelper = new SessionHelper(loggeMock.Object, _context);
        }

        [Test]
        [TestCase(SessionState.Active)]
        [TestCase(SessionState.NotAuthorized)]
        [TestCase(SessionState.Expired)]
        public void ShouldVerifySessionStatus(SessionState sessionState)
        {
            //ARANGE
            var sessioId = "123456";
            var customer = CustomerTestHelper.CreateCustomer(_context, firstName, lastName, username, email);
            var session = CreateSessionTestHelper.CreateSession(_context, sessioId, customer, sessionState);

            //ACT
            var sessionStatusResult = sessionHelper.CheckSessionStatus(session);

            //ASSERT
            sessionStatusResult.Should().Be(sessionState);
        }

        [Test]
        public void ShouldBeAbleVerifySessionIsExpired_true()
        {
            //ARRANGE
            var sessioId = "123456";
            var customer = CustomerTestHelper.CreateCustomer(_context, firstName, lastName, username, email);
            var session = CreateSessionTestHelper.CreateSession(_context, sessioId, customer, SessionState.Active, x =>
            {
                x.GenerationDate = DateTime.UtcNow.AddMinutes(-20);
                x.LastAccessDate = DateTime.UtcNow.AddMinutes(-10);
            });

            //ACT
            var sessionStatusResult = sessionHelper.SessionExpired(session);

            //ASSERT
            sessionStatusResult.Should().Be(true);
        }

        [Test]
        public void ShouldBeAbleVerifySessionIsExpired_False()
        {
            //ARRANGE
            var sessioId = "123456";
            var customer = CustomerTestHelper.CreateCustomer(_context, firstName, lastName, username, email);
            var session = CreateSessionTestHelper.CreateSession(_context, sessioId, customer, SessionState.Active);

            //ACT
            var sessionStatusResult = sessionHelper.SessionExpired(session);

            //ASSERT
            sessionStatusResult.Should().Be(false);
        }

        [Test]
        public void ShouldBeAbleValidateGivenSession_WhenSessionNotExist()
        {
            //ARRANGE
            var sessioId = "123456";
            var customer = CustomerTestHelper.CreateCustomer(_context, firstName, lastName, username, email);
            var session = CreateSessionTestHelper.CreateSession(_context, sessioId, customer, SessionState.Active);

            //ACT
            var exception = Assert.Throws<CoreException>(() => sessionHelper.ValidateSessionStatus(null));

            //ASSERT
            Assert.AreEqual(ErrorCode.SessionDoesNotExist, exception.Code);
        }

        [Test]
        public void ShouldReturnSessionNotAuthorized()
        {
            //ARRANGE
            var sessioId = "123456";
            var customer = CustomerTestHelper.CreateCustomer(_context, firstName, lastName, username, email);
            var session = CreateSessionTestHelper.CreateSession(_context, sessioId, customer, SessionState.NotAuthorized);

            //ACT
            var exception = Assert.Throws<CoreException>(() => sessionHelper.ValidateSessionStatus(session));

            //ASSERT
            exception.Code.Should().Be(ErrorCode.SessionNotAuthorized);
        }

        [Test]
        public void ShouldReturnSessionExpired()
        {
            //ARRANGE
            var sessioId = "123456";
            var customer = CustomerTestHelper.CreateCustomer(_context, firstName, lastName, username, email);
            var session = CreateSessionTestHelper.CreateSession(_context, sessioId, customer, SessionState.Expired);

            //ACT
            var exception = Assert.Throws<CoreException>(() => sessionHelper.ValidateSessionStatus(session));

            //ASSERT
            exception.Code.Should().Be(ErrorCode.SessionExpired);
        }

        [Test]
        public async Task ShouldBeAbleGetOnlyOneSession_WithGivenId()
        {
            //ARRANGE
            var sessioId = "123456";
            var customer = CustomerTestHelper.CreateCustomer(_context, firstName, lastName, username, email);
            CreateSessionTestHelper.CreateSession(_context, sessioId, customer, SessionState.Active);

            //ACT
            var currentSession = await sessionHelper.GetSessionByIdAsync(_context, sessioId);

            //ASSERT
            currentSession.Should().NotBeNull();
        }

        [Test]
        public async Task ShouldNotBeAbleGetSession_WrongGivenId()
        {
            //ARRANGE
            var sessioId = "123456";
            var wrongSessionId = "654321";
            var customer = CustomerTestHelper.CreateCustomer(_context, firstName, lastName, username, email);
            CreateSessionTestHelper.CreateSession(_context, sessioId, customer, SessionState.Active);

            //ACT
            var currentSession = await sessionHelper.GetSessionByIdAsync(_context, wrongSessionId);

            //ASSERT
            currentSession.Should().BeNull();
        }

        [Test]
        public async Task ShouldBeAbleCreateSession()
        {
            //ARRANGE
            var customer = CustomerTestHelper.CreateCustomer(_context, firstName, lastName, username, email);
            
            //ACT
            var session = await sessionHelper.CreateSession(customer);

            //ASSERT
            session.Should().NotBeNull();
            session.State.Should().Be(SessionState.NotAuthorized);
            session.IdCustomer.Should().Be(customer.CustomerId);
        }

        [Test]
        public async Task ShouldBeAbleRemoveSession()
        {
            //ARRANGE
            var customer = CustomerTestHelper.CreateCustomer(_context, firstName, lastName, username, email);
            var session = SessionTestHelper.CreateSession(_context, GetRandomSessionId(), customer, SessionState.Active);

            //ACT
            await sessionHelper.RemoveSession(session.SessionIdentifier);

            //ASSERT
            var findOldSession = SessionTestHelper.FindSession(_context, session.SessionIdentifier);
            findOldSession.Should().BeNull();
        }

        [Test]
        public void ShouldNotBeAbleRemoveSession_SessionNotExist()
        {
            //ARRANGE
            var customer = CustomerTestHelper.CreateCustomer(_context, firstName, lastName, username, email);
            var session = SessionTestHelper.CreateSession(_context, GetRandomSessionId(), customer, SessionState.Active);

            //ACT
            var exception = Assert.ThrowsAsync<CoreException>(() => sessionHelper.RemoveSession(GetRandomSessionId()));

            //ASSERT
            var createdSession = SessionTestHelper.FindSession(_context, session.SessionIdentifier);
            createdSession.Should().NotBeNull();

            exception.Code.Should().Be(ErrorCode.SessionDoesNotExist);
        }

        [Test]
        public void ShouldBeAbleRemoveAllSessions_BelongToGivenCustomer()
        {
            //ARRANGE
            var customer = CustomerTestHelper.CreateCustomer(_context, firstName, lastName, username, email);
            var sessionAuthorized = SessionTestHelper.CreateSession(_context, GetRandomSessionId(), customer, SessionState.Active);
            var sessionNotAuthorized = SessionTestHelper.CreateSession(_context, GetRandomSessionId(), customer, SessionState.NotAuthorized);

            //ACT
            sessionHelper.RemoveAllSession(customer.Username);

            //ASSERT
            var findSessionAuthorized = SessionTestHelper.FindAllCustomerSession(_context, customer.Username);
            findSessionAuthorized.Should().BeEmpty();
        }

        [Test]
        public void ShouldNotBeAbleRemoveAllSessions_WrongCustomer()
        {
            //ARRANGE
            var customer = CustomerTestHelper.CreateCustomer(_context, firstName, lastName, username, email);
            var sessionAuthorized = SessionTestHelper.CreateSession(_context, GetRandomSessionId(), customer, SessionState.Active);
            var sessionNotAuthorized = SessionTestHelper.CreateSession(_context, GetRandomSessionId(), customer, SessionState.NotAuthorized);

            //ACT
            sessionHelper.RemoveAllSession("wrongCustomer");

            //ASSERT
            var findSessionAuthorized = SessionTestHelper.FindAllCustomerSession(_context, customer.Username);
            findSessionAuthorized.Should().NotBeEmpty();
        }

        private string GetRandomSessionId()
        {
            var id = Guid.NewGuid().ToString();

            id = id.Replace("-", "");

            return id;
        }
    }
}
