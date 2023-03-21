using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Rental.Core.Enum;
using Rental.Core.Validation;
using Rental.Infrastructure.DTO;
using Rental.Infrastructure.Exceptions;
using Rental.Infrastructure.Helpers;
using Rental.Infrastructure.Query;
using Rental.Infrastructure.Wrapper;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Rental.Infrastructure.Handlers.Account.Query.AccountDetails
{
    public class GetCustomerDetailsHandler : IQueryHandler<GetCustomerDetailsRequest, GetCustomerDetailsResponse>
    {
        private readonly ILogger<GetCustomerDetailsHandler> _logger;
        private readonly ICustomerHelper _customerHelper;
        private readonly IHttpContextWrapper _httpContextWrapper;
        private readonly ISessionHelper _sessionHelper;

        public GetCustomerDetailsHandler(ILogger<GetCustomerDetailsHandler> logger, ICustomerHelper customerHelper, IHttpContextWrapper httpContextWrapper, 
            ISessionHelper sessionHelper)
        {
            _logger = logger;
            _customerHelper = customerHelper;
            _httpContextWrapper = httpContextWrapper;
            _sessionHelper = sessionHelper;
        }

        public async ValueTask<GetCustomerDetailsResponse> HandleAsync([NotNull] GetCustomerDetailsRequest command, CancellationToken cancellationToken = default)
        {
            ValidationParameter.FailIfNull(command);

            CustomerAccountDto customerAccountDto;
            try
            {
                var sessionId = _httpContextWrapper.GetValueFromRequestHeader("SessionId");
                var clientSession = await _sessionHelper.GetSessionByIdAsync(sessionId);

                if (_sessionHelper.CheckSessionStatus(clientSession) != SessionState.Active)
                    throw new CoreException(ErrorCode.SessioNotActive, $"Session is not active.");

                var customer = await _customerHelper.GetCustomerAsync(command.Username);

                customerAccountDto = new CustomerAccountDto
                {
                    Fullname = customer.FirstName + " " + customer.LastName,
                    Username = customer.Username,
                    Email = customer.Email,
                    Status = customer.Status,
                    PasswordStatus = customer.Passwords.First().Status
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Customer with name {command.Username} does not exist." + $" {ex.Message}");
                throw;
            }

            return new GetCustomerDetailsResponse
            {
                Fullname = customerAccountDto.Fullname,
                Username = customerAccountDto.Username,
                Email = customerAccountDto.Email,
                Status = customerAccountDto.Status.ToString(),
                PasswordStatus = customerAccountDto.PasswordStatus.ToString()
            };
        }
    }
}
