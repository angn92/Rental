using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Rental.Core.Domain;
using Rental.Core.Enum;
using Rental.Infrastructure.EF;
using Rental.Infrastructure.Exceptions;
using Rental.Infrastructure.Helpers;
using System;
using System.Threading.Tasks;

namespace Rental.Infrastructure.Services.CustomerService
{
    public class CustomerService : ICustomerService
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailHelper _emailValidator;
        private readonly IPasswordHelper _passwordHelper;

        public CustomerService(ApplicationDbContext context, IEmailHelper emailValidator, IPasswordHelper passwordHelper)
        {
            _context = context;
            _emailValidator = emailValidator;
            _passwordHelper = passwordHelper;
        }

        /// <summary>
        /// Get details customer from database with given username in argument 
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public async Task<Customer> GetCustomerAsync([NotNull] string username)
        {
            var customer = await _context.Customers.SingleOrDefaultAsync(x => x.Username == username);

            if (customer is null)
                throw new CoreException(ErrorCode.UserNotExist, $"User {username} does not exist.");

            return customer;
        }

        public Task<Customer> LoginAsync([NotNull] string username, [NotNull] string password)
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
        /// <returns></returns>
        public async Task<Customer> RegisterAsync([NotNull] string firstName, [NotNull] string lastName, [NotNull] string username,
                                        [NotNull] string email, [CanBeNull] string phoneNumber)
        {
            Customer customer;

            try
            {
                _emailValidator.ValidateEmail(email);
                customer = new Customer(firstName, lastName, username, email, phoneNumber);
                await _context.AddAsync(customer);
            }
            catch (Exception ex)
            {
                throw new Exception("Registration is failed. " + ex.Message);
            }

            await _context.SaveChangesAsync();

            return customer;
        }

        public async Task<bool> CheckIfExist([NotNull] string username)
        {
            var customer = await _context.Customers.SingleOrDefaultAsync(x => x.Username == username);

            if (customer is null)
                return false;

            return true;
        }

        public void ValidateCustomerAccountAsync(Customer customer)
        {
            if (customer.Status == AccountStatus.Blocked)
                throw new CoreException(ErrorCode.AccountBlocked, "Account is blocked");

            if (customer.Status == AccountStatus.NotActive)
                throw new CoreException(ErrorCode.AccountNotActive, "Account is not active");
        }
    }
}
