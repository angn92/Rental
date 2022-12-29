using JetBrains.Annotations;
using Rental.Core.Domain;
using Rental.Core.Enum;
using Rental.Infrastructure.EF;
using Rental.Infrastructure.Helpers;
using System;
using System.Threading.Tasks;

namespace Rental.Infrastructure.Services.CustomerService
{
    public class CustomerService : ICustomerService
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailHelper _emailValidator;

        public CustomerService(ApplicationDbContext context, IEmailHelper emailValidator)
        {
            _context = context;
            _emailValidator = emailValidator;
        }

        public async Task<Customer> RegisterAsync([NotNull] string firstName, [NotNull] string lastName, [NotNull] string username, 
            [NotNull] string email)
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

        public void ChangeAccountStatus([NotNull] Customer customer, AccountStatus status)
        {
            customer.Status = status;
        }
    }
}
