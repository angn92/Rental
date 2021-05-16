using FluentAssertions;
using Moq;
using NUnit.Framework;
using Rental.Core.Domain;
using Rental.Core.Enum;
using Rental.Infrastructure.Services.SessionService;
using System.Threading.Tasks;

namespace Rental.UnitTest.Services
{
    [TestFixture]
    public class SessionTest
    {
        [Test]
        public void ShouldBeReturnSession()
        {
            var sessionMock = new Mock<SessionService>();  //Mock object
            var session = new Mock<Session>();

            var result = sessionMock.Setup(x => x.CreateSessionAsync(It.IsAny<User>()))
                                    .Returns(Task.FromResult<Session>(session.Object));

            result.Should().NotBeNull();
        }
    }
}
