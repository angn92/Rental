using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Rental.Core.Domain;
using Rental.Core.Enum;
using Rental.Infrastructure.EF;
using Rental.Infrastructure.Exceptions;
using Rental.Infrastructure.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Rental.Infrastructure.Helpers
{
    public interface ICustomerHelper : IService
    {
        Task<Customer> RegisterAsync([NotNull] string firstName, [NotNull] string lastName, [NotNull] string username, [NotNull] string email);
        string CheckAccountStatus([NotNull] Customer user);
        Task<Customer> GetCustomerAsync([NotNull] string username);
        void ValidateCustomerAccount([NotNull] Customer customer);
        bool CheckIfExist([NotNull] string username);
        void ChangeAccountStatus([NotNull] Customer customer, AccountStatus status);
    }

    public class CustomerHelper : ICustomerHelper
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailHelper _emailValidator;

        public CustomerHelper(ApplicationDbContext context, IEmailHelper emailValidator)
        {
            _context = context;
            _emailValidator = emailValidator;
        }

        public async Task<Customer> RegisterAsync([NotNull] string firstName, [NotNull] string lastName, [NotNull] string username, [NotNull] string email)
        {
            Customer customer;
            try
            {
                _emailValidator.ValidateEmail(email);
                customer = new Customer(firstName, lastName, username, email);
                await _context.AddAsync(customer);
            }
            catch (Exception ex)
            {
                throw new Exception("Registration is failed. " + ex.Message);
            }

            await _context.SaveChangesAsync();

            return customer;
        }

        public string CheckAccountStatus([NotNull] Customer customer)
        {
            string status = null;

            switch (customer.Status)
            {
                case AccountStatus.Active:
                    status = AccountStatus.Active.ToString();
                    break;
                case AccountStatus.Blocked:
                    status = AccountStatus.Blocked.ToString();
                    break;
                case AccountStatus.NotActive:
                    status = AccountStatus.NotActive.ToString();
                    break;
            }
            return status;
        }

        public async Task<Customer> GetCustomerAsync([NotNull] string username)
        {
            var customer = await _context.Customers.Include("Passwords").SingleOrDefaultAsync(x => x.Username == username);

            if (customer is null)
                throw new CoreException(ErrorCode.UserNotExist, $"User {username} does not exist.");

            return customer;
        }

        public void ValidateCustomerAccount([NotNull] Customer customer)
        {
            if (customer.Status == AccountStatus.Blocked)
                throw new CoreException(ErrorCode.AccountBlocked, $"Account for customer {customer.Username} is blocked.");

            if (customer.Status == AccountStatus.NotActive)
                throw new CoreException(ErrorCode.AccountNotActive, $"Account for customer {customer.Username} is not active.");
        }

        public bool CheckIfExist([NotNull] string username)
        {
            var customer = _context.Customers.SingleOrDefault(x => x.Username == username);

            if (customer is null)
                return false;

            return true;
        }
        
        public void ChangeAccountStatus([NotNull] Customer customer, AccountStatus status)
        {
            customer.Status = status;
        }
    }
}
