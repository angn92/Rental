using JetBrains.Annotations;
using Rental.Core.Enum;
using System;
using System.Collections.Generic;

namespace Rental.Core.Domain
{
    public class User : Entity
    {
        public string FirstName { get; protected set; }
        public string LastName { get; protected set; }
        public string Username { get; protected set; }
        public string Email { get; protected set; }
        public AccountStatus Status { get; protected set; }
        public string Phone { get; protected set; }
        public ISet<Password> Passwords { get; protected set; }
        public ISet<Product> Products { get; protected set; }
        public Session SessionId { get; protected set; }

        protected User()
        {
        }

        public User([NotNull] string firstName, [NotNull] string lastName, [NotNull] string username, [NotNull] string email, [NotNull] string phone)
        {
            SetFirstName(firstName);
            SetLastName(lastName);
            SetUserame(username);
            SetEmail(email);
            Status = AccountStatus.Active;
            Phone = phone;
        }

        public void SetFirstName(string firstName)
        {
            if (String.IsNullOrWhiteSpace(firstName))
                throw new Exception($"FirstName parameter is incorrect.");

            FirstName = firstName;
        }

        public void SetLastName(string lastName)
        {
            if (String.IsNullOrWhiteSpace(lastName))
                throw new Exception($"Argument {lastName} is incorrect.");

            LastName = lastName;
        }

        public void SetPhoneNumber(string number)
        {
            if (String.IsNullOrWhiteSpace(number))
                throw new Exception($"Argumnet {number} can not be null or empty.");

            Phone = number;
        }

        public void SetUserame(string username)
        {
            if (String.IsNullOrWhiteSpace(username))
                throw new Exception($"Argument {username} is invalid.");

            Username = username;
        }

        public void SetEmail(string email)
        {
            if (String.IsNullOrWhiteSpace(email))
                throw new Exception($"Argument {email} is invalid.");

            Email = email;
        }

        public void SetActiveAccount(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user), "Argument can not be null.");

            user.Status = AccountStatus.Active;
        }

        public void SetBlockade(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user), "Argument can not be null.");

            user.Status = AccountStatus.Blocked;
        }
    }
}
