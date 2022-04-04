using FluentAssertions;
using Moq;
using NUnit.Framework;
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
            var customer = CreateCustomerTestHelper.CreateActiveCustomer(GetContext(), firstName, lastName, userName, email, password);

            var emailMock = new Mock<IEmailHelper>();
            var passwordMock = new Mock<IPasswordHelper>();

            var customerService = new CustomerService(GetContext(), emailMock.Object, passwordMock.Object);
            var userHelper = new UserHelper();
            var sessionService = new SessionService(GetContext(), customerService, userHelper);
            var handler = new CreateSessionHandler(customerService, sessionService);

            // ACT
            var result = await handler.HandleAsync(new CreateSessionCommand 
            { 
                Username = userName 
            },
            CancellationToken.None);

            // ASSERT
            //result.IdSession.Should().NotBeNull();
        }
    }
}
