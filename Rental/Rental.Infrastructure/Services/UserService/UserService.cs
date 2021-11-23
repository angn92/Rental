using Microsoft.EntityFrameworkCore;
using Rental.Core.Domain;
using Rental.Infrastructure.EF;
using Rental.Infrastructure.Exceptions;
using Rental.Infrastructure.Helpers;
using System;
using System.Threading.Tasks;

namespace Rental.Infrastructure.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailValidator _emailValidator;
        private readonly IPasswordHelper _passwordHelper;

        public UserService(ApplicationDbContext context, IEmailValidator emailValidator, IPasswordHelper passwordHelper)
        {
            _context = context;
            _emailValidator = emailValidator;
            _passwordHelper = passwordHelper;
        }

        /// <summary>
        /// Get customer from database with given nick like in argument 
        /// </summary>
        /// <param name="nick"></param>
        /// <returns></returns>
        public async Task<Customer> GetCustomerAsync(string nick)
        {
            var customer = await _context.Customers.SingleOrDefaultAsync(x => x.Username == nick);

            if (customer is null)
                throw new CoreException(ErrorCode.UserNotExist, $"This user {nick} does not exist.");

            return customer;
        }

        public Task<Customer> LoginAsync(string username, string password)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Register new customer
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="username"></param>
        /// <param name="email"></param>
        /// <param name="phoneNumber"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task RegisterAsync(string firstName, string lastName, string username, string email, 
                                        string phoneNumber, string password)
        {
            var customer = await _context.Customers.SingleOrDefaultAsync(x => x.Username == username);

            if (customer != null)
            {
                throw new CoreException(ErrorCode.UsernameExist, $"Username {customer.Username} already exist.");
            }

            try
            {
                _emailValidator.ValidateEmail(email);
                customer = new Customer(firstName, lastName, username, email, phoneNumber);
                await _context.AddAsync(customer);

                //_passwordHelper.SetPassword(password, user);
            }
            catch (Exception ex)
            {
                throw new Exception("Registration is failed. " + ex.Message);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<bool> CheckIfExist(string username)
        {
            var customer = await _context.Customers.SingleOrDefaultAsync(x => x.Username == username);

            if (customer is null)
                return false;

            return true;
        }
    }
}
