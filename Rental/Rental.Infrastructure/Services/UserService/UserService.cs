using Rental.Core.Domain;
using Rental.Core.Repository;
using Rental.Infrastructure.DTO;
using Rental.Infrastructure.Exceptions;
using System;
using System.Threading.Tasks;

namespace Rental.Infrastructure.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<UserDto> LoginAsync(string username, string password)
        {
            throw new NotImplementedException();
        }

        public async Task RegisterAsync(string firstName, string lastName, string username, string email, string phoneNumber)
        {
            var user = await _userRepository.GetAsync(username);

            if (user != null)
            {
                throw new CoreException(ErrorCode.UsernameExist, $"Username {user.Username} already exist.");
            }

            user = new Account(firstName, lastName, username, email, phoneNumber);
            await _userRepository.AddAsync(user);
        }
    }
}
