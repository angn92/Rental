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
        public virtual string CustomerId { get; protected set; }
        public virtual string FirstName { get; protected set; }
        public virtual string LastName { get; protected set; }
        public virtual string Username { get; protected set; }
        public virtual string Email { get; protected set; }
        public virtual AccountStatus Status { get; protected set; }
        public virtual string Phone { get; protected set; }
        public virtual ISet<Password> Passwords { get; protected set; }
        public virtual ISet<Product> Products { get; protected set; }
        public virtual Session Session { get; protected set; }

        protected Customer()
        {
        }

        public Customer([NotNull] string firstName, [NotNull] string lastName, [NotNull] string username, 
                    [NotNull] string email, [NotNull] string phone)
        {
            CustomerId = Guid.NewGuid().ToString();
            SetFirstName(firstName);
            SetLastName(lastName);
            SetUsername(username);
            SetEmail(email);
            SetPhone(phone);
            Status = AccountStatus.Active;
        }

        public void SetFirstName(string firstName)
        {
            ValidationParameter.FailIfNullOrEmpty(firstName);
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
