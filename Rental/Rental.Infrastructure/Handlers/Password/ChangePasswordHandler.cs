using Rental.Core.Enum;
using Rental.Infrastructure.Command;
using Rental.Infrastructure.EF;
using Rental.Infrastructure.Exceptions;
using Rental.Infrastructure.Helpers;
using Rental.Infrastructure.Services.SessionService;
using Rental.Infrastructure.Services.UserService;
using System.Threading.Tasks;

namespace Rental.Infrastructure.Handlers.Password
{
    public class ChangePasswordHandler : ICommandHandler<ChangePasswordCommand>
    {
        private readonly ApplicationDbContext _rentalContext;
        private readonly IUserService _userService;
        private readonly ISessionService _sessionService;
        private readonly ISessionHelper _sessionHelper;
        private readonly IPasswordHelper _passwordHelper;

        public ChangePasswordHandler(ApplicationDbContext rentalContext, IUserService userService, ISessionService sessionService,
                                     ISessionHelper sessionHelper, IPasswordHelper passwordHelper)
        {
            _rentalContext = rentalContext;
            _userService = userService;
            _sessionService = sessionService;
            _sessionHelper = sessionHelper;
            _passwordHelper = passwordHelper;
        }

        public async Task HandleAsync(ChangePasswordCommand command)
        {
            var session = await _sessionService.GetSessionAsync(command.Session);

            if (session == null)
            {
                throw new CoreException(ErrorCode.SessionDoesNotExist, $"Session {command.Session} does not exist.");
            }

            _sessionHelper.CheckSessionStatus(session);

            //var customer = await _userService.GetCustomerAsync(session.Customer.Username);

            //if(customer.Status != AccountStatus.Active)
            //{
            //    throw new CoreException(ErrorCode.AccountNotActive, "Only active user can change password.");
            //}

            if (_sessionHelper.SessionExpired(session))
            {
                throw new CoreException(ErrorCode.SessionExpired, $"Session {session.SessionId} is expired.");
            }

            //Remove old password form DB
            //var oldPassword = await _passwordHelper.GetActivePassword(customer);

            //_rentalContext.Remove(oldPassword);
            await _rentalContext.SaveChangesAsync();

            //Set new password and save to DB

            //await _passwordHelper.SetPassword(command.NewPassword, customer);
        }
    }
}
