using Autofac;
using Moq;
using NUnit.Framework;
using Rental.Core.Enum;
using Rental.Infrastructure.Command;
using Rental.Infrastructure.EF;
using Rental.Infrastructure.Handlers.Sessions;
using Rental.Test.Helpers;
using System.Threading;
using System.Threading.Tasks;

namespace Rental.Test.Invocation.Session
{
    [TestFixture]
    public class CreateSessionTest : BaseServiceTest
    {
        private ApplicationDbContext context;
        private string firstName, lastName, userName, email, phone, password;
        private readonly ICommandHandler<CreateSessionCommand> commandHandler;

        [SetUp]
        public void InitDatabase()
        {
            context = GetContext();
            firstName = "Jan";
            lastName = "Nowak";
            userName = "username";
            email = "email@email.com";
            phone = "123456789";
            password = "pass";
        }

        [Test]
        public async Task ShouldBeAbleCreateNewSession()
        {
            //ASSERT
            var sessioId = "123456";
            var customer = CreateCustomerTestHelper.CreateActiveCustomer(context, firstName, lastName, userName, email, password);
            var session = CreateSessionTestHelper.CreateActiveSession(context, sessioId, customer);

            var command = new CreateSessionCommand
            {
                Username = customer.Username
            };

            
            
            var dispatcher = new CommandDispatcher(context);


            await dispatcher.DispatchAsync<CreateSessionCommand, CreateSessionResponse>(command);
            
            //ACT
            //await _commandDispatcher.DispatchAsync<CreateSessionCommand>(command);

            //ARRANGE
        }
    }
}
