using JetBrains.Annotations;
using Rental.Core.Base;
using Rental.Core.Enum;
using Rental.Core.Validation;
using System;
using System.Collections.Generic;

namespace Rental.Core.Domain
{
    public class Customer : Entity
    {
        public virtual int CustomerId { get; set; }
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

        public Customer([NotNull] string firstName, [NotNull] string lastName, [NotNull] string username, [NotNull] string email, 
            [NotNull] string phone)
        {
            SetFirstName(firstName);
            SetLastName(lastName);
            SetUsername(username);
            SetEmail(email);
            SetPhone(phone);
            Status = AccountStatus.Active;
        }

        public void SetFirstName(string firstName)
        {
            ValidationParameter.FailIfNullOrEmpty(firstName, nameof(FirstName));
            FirstName = firstName;
        }

        public void SetLastName(string lastName)
        {
            ValidationParameter.FailIfNullOrEmpty(lastName);
            LastName = lastName;
        }

        public void SetUsername(string username)
        {
            ValidationParameter.FailIfNullOrEmpty(username);
            Username = username;
        }

        public void SetEmail(string email)
        {
            Email = email;
        }

        public void SetPhone(string phoneNumber)
        {
            Phone = phoneNumber;
        }

        public void ActivateAccount(Customer customer)
        {
            customer.Status = AccountStatus.Active;
        }

        public void BlockadeAccount(Customer customer)
        {
            customer.Status = AccountStatus.Blocked;
        }
    }
}
