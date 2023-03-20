using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Rental.Core.Enum;
using Rental.Infrastructure.Handlers.Account.Query.AccountDetails;
using Rental.Infrastructure.Query;
using Rental.Test.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rental.Test.IntegrationTest.Account
{
    [TestFixture]
    public class CustomerDetailsTest : WebApplicationFactoryBase
    {
        [Test]
        public void CustomerShouldBeAbleDisplayOwnAccountDetails()
        {
            //ARRANGE
            var customer = CustomerTestHelper.CreateCustomer(_context, "Shane", "Andersen", "shane_andersen", "shane_andersen@email.com");
            var session = SessionTestHelper.CreateSession(_context, "1234554321", customer, SessionState.Active);

            var request = new GetCustomerDetailsRequest
            {
                Username = customer.Username
            };

            //var httpContext = new DefaultHttpContext();
            //httpContext.Request.Headers["SessionId"] = session.SessionIdentifier;

            //ACT
            var result = _factory.Services.GetRequiredService<IQueryHandler<GetCustomerDetailsRequest, GetCustomerDetailsResponse>>().HandleAsync(request);

        }

        
    }
}
