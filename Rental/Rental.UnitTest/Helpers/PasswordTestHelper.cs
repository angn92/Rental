using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Rental.Core.Domain;
using Rental.Infrastructure.EF;
using System;
using System.Linq;

namespace Rental.Test.Helpers
{
    public static class PasswordTestHelper
    {
        public static Password CreatePassword([NotNull] ApplicationDbContext context, string hash, string salt, 
            Customer customer, string code, Action<Password> action = null)
        {
            var password = new Password(hash, salt, customer, code);

            action?.Invoke(password);

            context.Add(password);
            context.SaveChanges();

            return password;
        }

        public static Password FindPasswordById([NotNull] ApplicationDbContext context, string passwordId)
        {
            return context.Passwords.FirstOrDefault(x => x.PasswordId == passwordId);
        }

        public static Password FindPasswordByUsername([NotNull] ApplicationDbContext context, string username)
        {
            return context.Passwords.SingleOrDefault(x => x.Customer.Username == username);
        }
    }
}
