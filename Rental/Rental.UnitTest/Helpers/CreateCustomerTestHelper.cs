using JetBrains.Annotations;
using Rental.Core.Domain;
using Rental.Infrastructure.EF;
using System;

namespace Rental.Test.Helpers
{
    public static class CreateCustomerTestHelper
    {
        public static Customer CreateActiveCustomer([NotNull] ApplicationDbContext context, [NotNull] string firstName, [NotNull] string lastName, 
            [NotNull] string username, [NotNull] string email, [NotNull] string password, Action<Customer> action = null)
        {
            var customer = new Customer(firstName, lastName, username, email, password);

            action?.Invoke(customer);

            context.Add(customer);
            context.SaveChanges();

            return customer;
        }
    }
}