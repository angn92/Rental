using Rental.Core.Domain;
using Rental.Core.Repository;
using Rental.Infrastructure.DTO;
using Rental.Infrastructure.Exceptions;
using Rental.Infrastructure.Helpers;
using System;
using System.Threading.Tasks;

namespace Rental.Infrastructure.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailValidator _emailValidator;
        private readonly IPasswordHelper _passwordHelper;

        public UserService(IUserRepository userRepository, IEmailValidator emailValidator, IPasswordHelper passwordHelper)
        {
            _userRepository = userRepository;
            _emailValidator = emailValidator;
            _passwordHelper = passwordHelper;
        }

        public async Task<UserDto> GetUserAsync(string nick)
        {
            var user = await _userRepository.GetAsync(nick);

            if (user is null)
                throw new CoreException(ErrorCode.UserNotExist, $"This user: {nick} does not exist.");

            var userDetails = new UserDto
            {
                FullName = user.FirstName + " " + user.LastName,
                Emial = user.Email,
                Username = user.Username,
                Status = user.Status
            };

            return userDetails;
        }

        public Task<UserDto> LoginAsync(string username, string password)
        {
            throw new NotImplementedException();
        }

        public async Task RegisterAsync(string firstName, string lastName, string username, string email, string phoneNumber, string password)
        {
            var user = await _userRepository.GetAsync(username);

            if (user != null)
            {
                throw new CoreException(ErrorCode.UsernameExist, $"Username {user.Username} already exist.");
            }

            try
            {
                _emailValidator.ValidateEmail(email);
                user = new User(firstName, lastName, username, email, phoneNumber);
                await _userRepository.AddAsync(user);

                //_passwordHelper.SetPassword(password, user);
            }
            catch (Exception ex)
            {
                throw new Exception("Registration is failed. " + ex.Message);
            }
        }
    }
}
