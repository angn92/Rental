﻿using JetBrains.Annotations;
using Rental.Core.Validation;
using Rental.Infrastructure.Query;
using Rental.Infrastructure.Services.CustomerService;
using System.Threading;
using System.Threading.Tasks;

namespace Rental.Infrastructure.Handlers.Account.Query.AccountDetails
{
    public class GetCustomerDetailsHandler : IQueryHandler<GetCustomerDetailsRq, GetCustomerDetailsRs>
    {
        private readonly ICustomerService _customerService;

        public GetCustomerDetailsHandler(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public async ValueTask<GetCustomerDetailsRs> HandleAsync([NotNull] GetCustomerDetailsRq command, CancellationToken cancellationToken = default)
        {
            ValidationParameter.FailIfNull(command);

            var customerAccount = await _customerService.GetCustomerAsync(command.Username);

            return new GetCustomerDetailsRs
            {
                Fullname = customerAccount.FirstName + " " + customerAccount.LastName,
                Username = customerAccount.Username,
                Email = customerAccount.Email,
                Status = customerAccount.Status
            };
        }
    }
}