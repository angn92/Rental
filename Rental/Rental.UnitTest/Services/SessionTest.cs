using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using Rental.Core.Domain;
using Rental.Core.Enum;
using Rental.Infrastructure.EF;
using Rental.Infrastructure.Helpers;
using Rental.Infrastructure.Services.SessionService;
using Rental.Infrastructure.Services.UserService;
using System.Threading.Tasks;

namespace Rental.UnitTest.Services
{
    [TestFixture]
    public class SessionTest
    {
        [Test]
        public void ShouldBeAbleInvoke_CreateSession()
        {
            var sessionServiceMock = new Mock<ISessionService>();  //Mock object
            var sessionMock = new Mock<Session>();

            var result = sessionServiceMock.Setup(x => x.CreateSessionAsync(It.IsAny<User>()))
                                    .Returns(Task.FromResult<Session>(sessionMock.Object));
          
            result.Should().NotBeNull();
        }

        [Test]
        public void ShouldBeReturnIdSession()
        {
            var sessionServiceMock = new Mock<ISessionService>();  //Mock object
            var contextMock = new Mock<RentalContext>();
            var userHelperMock = new Mock<IUserHelper>();
            var userServiceMock = new Mock<IUserService>();

            //var sessionMock = new Mock<Session>();
            var user = new User("test", "test1", "userTest", "test@email.com", "123456789");
            //var session = new Session("1234567", SessionState.Active, user);

            contextMock.Setup(x => x.Sessions.Add(It.IsAny<Session>())).Returns();
            //sessionServiceMock.Setup(x => x.CreateSessionAsync(It.IsAny<User>()))
            //                  .ReturnsAsync((Session s) => s);

            var sessionService = new SessionService(contextMock.Object, userServiceMock.Object, userHelperMock.Object);
            var session = sessionService.CreateSessionAsync(user);

            session.Should().NotBeNull();
            
        }
    }
}
