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
        private readonly RentalContext _rentalContext;
        private readonly IUserService _userService;
        private readonly ISessionService _sessionService;
        private readonly ISessionHelper _sessionHelper;

        public ChangePasswordHandler(RentalContext rentalContext, IUserService userService, ISessionService sessionService,
                                     ISessionHelper sessionHelper)
        {
            _rentalContext = rentalContext;
            _userService = userService;
            _sessionService = sessionService;
            _sessionHelper = sessionHelper;
        }

        public async Task HandleAsync(ChangePasswordCommand command)
        {
            var session = await _sessionService.GetSessionAsync(command.Session);

            if (session == null)
            {
                throw new CoreException(ErrorCode.SessionDoesNotExist, $"Session {command.Session} does not exist.");
            }

            _sessionHelper.CheckSessionStatus(session);

            var user = await _userService.GetUserAsync(session.User.Username);

            if(user.Status != AccountStatus.Active)
            {
                throw new CoreException(ErrorCode.AccountNotActive, "Only active user can change password.");
            }

            
            //check session for user, session should not be expire
            //validate new password
            //save password 
            //return 200 if is ok 
        }
    }
}
