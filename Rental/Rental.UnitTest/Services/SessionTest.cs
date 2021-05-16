using FluentAssertions;
using Moq;
using NUnit.Framework;
using Rental.Core.Domain;
using Rental.Core.Enum;
using Rental.Infrastructure.Helpers;
using System.Threading.Tasks;

namespace Rental.UnitTest.Services
{
    [TestFixture]
    public class SessionTest
    {
        [Test]
        public void ShouldBeReturnSession()
        {
            var sessionMock = new Mock<ISessionHelper>();  //Mock object
            var session = new Mock<Session>();

            var result = sessionMock.Setup(x => x.CreateSession(It.IsAny<User>()))
                                    .Returns(Task.FromResult<Session>(session.Object));

            result.Should().NotBeNull();
        }

        [Test]
        public void ShouldBeReturnNewIdSession()
        {
            var user = new User("Adam", "Nowak", "adam00", "adam00@email.com", "123456789");
            var sessionHandlerMock = new Mock<ISessionHelper>();
            var sessionMock = new Mock<Session>();
            var session = new Session("12345", SessionState.Active, user);
            

            var s = sessionHandlerMock.Setup(x => x.CreateSession(user))
                                      .Returns(Task.FromResult<Session>(sessionMock.Object));

            Assert.AreEqual(session.SessionId, sessionHandlerMock.Verify<Session>(x => x.GetSessionAsync();
        }
    }
}
