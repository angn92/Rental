using FluentAssertions;
using Moq;
using NUnit.Framework;
using Rental.Core.Enum;
using Rental.Infrastructure.Handlers.Sessions;
using Rental.Infrastructure.Helpers;
using Rental.Infrastructure.Services.CustomerService;
using Rental.Infrastructure.Services.SessionService;
using Rental.Test.Helpers;
using System.Threading;
using System.Threading.Tasks;

namespace Rental.Test.Invocation.Session
{
    [TestFixture]
    public class CreateSessionTest : TestBase
    {
        [Test]
        public async Task ShouldBeAbleCreateNewSession()
        {
            // ARRANGE
            CreateCustomerTestHelper.CreateActiveCustomer(GetContext(), firstName, lastName, userName, email, password);

            var emailMock = new Mock<IEmailHelper>();
            var passwordMock = new Mock<IPasswordHelper>();

            var customerService = new CustomerService(GetContext(), emailMock.Object, passwordMock.Object);
            var userHelper = new UserHelper();
            var sessionService = new SessionService(GetContext(), customerService, userHelper);
            
            var handler = new CreateSessionHandler(GetContext(), customerService, sessionService);

            var command = new CreateSessionCommand
            {
                Username = userName
            };

            // ACT
            var result = await handler.HandleAsync(command, CancellationToken.None);

            // ASSERT
            Assert.AreEqual(SessionState.Active, result.Status);
        }
    }
}
