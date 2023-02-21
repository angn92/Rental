using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Rental.Infrastructure.Command;
using Rental.Infrastructure.Handlers.Account.Command.CreateAccount;

namespace Rental.Test.IntegrationTest.Account
{
    [TestFixture]
    public class RegisterCustomerTest : WebApplicationFactoryBase
    {
        [Test]
        public void CreateCustomer()
        {
            var command = new RegisterCustomer
            {
                FirstName = "Andrzej",
                LastName = "Gnutek",
                Username = "anderios",
                Email = "and@email.com",
                Password = "pass"
            };

            _factory.Services.GetRequiredService<ICommandHandler<RegisterCustomer, RegisterCustomerResponse>>().HandleAsync(command);

        }
    }
}
