using AdministartionConsole.Models.ViewModel;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Rental.Infrastructure.EF;

namespace AdministartionConsole.Helpers
{
    public interface ICustomerDtoHelper
    {
        Task<List<CustomerViewModel>> GetCustomerViews();
    }

    public class CustomerDtoHelper : ICustomerDtoHelper
    {
        private readonly ApplicationDbContext _context;

        public CustomerDtoHelper(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<CustomerViewModel>> GetCustomerViews()
        {
            return await _context.Customers.Join(
                _context.Passwords,
                c => c.CustomerId,
                p => p.Customer.CustomerId,
                (c, p) => new CustomerViewModel
                {
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Username = c.Username,
                    AccountStatus = c.Status.ToString(),
                    PasswordStatus = p.Status.ToString(),
                    CreatedDate = c.CreatedAt.ToString("G")
                }).ToListAsync();
        }
    }
}
