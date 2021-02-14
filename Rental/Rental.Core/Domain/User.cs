using JetBrains.Annotations;
using Rental.Core.Enum;
using System;
using System.Collections.Generic;

namespace Rental.Core.Domain
{
    public class User : Entity
    {
        [NotNull]
        public string FirstName { get; protected set; }

        [NotNull]
        public string LastName { get; protected set; }

        [NotNull]
        public string Username { get; protected set; }

        [NotNull]
        public string Email { get; protected set; }

        [NotNull]
        public AccountStatus Status { get; protected set; }

        [NotNull]
        public string Phone { get; protected set; }
        public DateTime CreatedAt { get; protected set; }
        public DateTime UpdatedAt { get; protected set; }
        public ISet<Password> Passwords { get; protected set; }
        public ISet<Product> Products { get; protected set; }

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
            CreatedAt = DateTime.UtcNow;
            Phone = phone;
        }

        public void SetFirstName(string firstName)
        {
            if (String.IsNullOrWhiteSpace(firstName))
                throw new Exception($"Argument {firstName} is incorrect.");

            FirstName = firstName;
            UpdateDate();
        }

        public void SetLastName(string lastName)
        {
            if (String.IsNullOrWhiteSpace(lastName))
                throw new Exception($"Argument {lastName} is incorrect.");

            LastName = lastName;
            UpdateDate();
        }

        public void SetPhoneNumber(string number)
        {
            if (String.IsNullOrWhiteSpace(number))
                throw new Exception($"Argumnet {number} can not be null or empty.");

            Phone = number;
            UpdateDate();
        }

        public void SetUserame(string username)
        {
            if (String.IsNullOrWhiteSpace(username))
                throw new Exception($"Argument {username} is invalid.");

            Username = username;
            UpdateDate();
        }

        public void SetEmail(string email)
        {
            if (String.IsNullOrWhiteSpace(email))
                throw new Exception($"Argument {email} is invalid.");

            Email = email;
            UpdateDate();
        }

        public void SetActiveAccount(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user), "Argument can not be null.");

            user.Status = AccountStatus.Active;
            UpdateDate();
        }

        public void SetBlockade(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user), "Argument can not be null.");

            user.Status = AccountStatus.Blocked;
            UpdateDate();
        }

        private void UpdateDate()
        {
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
