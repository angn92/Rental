using JetBrains.Annotations;
using Rental.Core.Enum;
using System;
using System.Collections.Generic;

namespace Rental.Core.Domain
{
    public class Customer : Entity
    {
        public virtual string CustomerId { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string Username { get; set; }
        public virtual string Email { get; set; }
        public virtual AccountStatus Status { get; set; }
        public virtual string Phone { get; set; }
        public virtual ISet<Password> Passwords { get; set; }
        public virtual ISet<Product> Products { get; set; }
        public virtual Session Session { get; set; }

        protected Customer()
        {
        }

        public Customer([NotNull] string firstName, [NotNull] string lastName, [NotNull] string username, 
                    [NotNull] string email, [NotNull] string phone)
        {
            CustomerId = Guid.NewGuid().ToString();
            FirstName = firstName;
            LastName = lastName;
            Username = username;
            Email = email;
            Phone = phone;
            Status = AccountStatus.NotActive;
        }

        public void ActivateAccount(Customer user)
        {
            user.Status = AccountStatus.Active;
        }

        public void BlockadeAccount(Customer user)
        {
            user.Status = AccountStatus.Blocked;
        }
    }
}
