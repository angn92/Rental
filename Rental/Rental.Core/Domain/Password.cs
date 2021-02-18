using JetBrains.Annotations;
using Rental.Core.Enum;
using System;

namespace Rental.Core.Domain
{
    public class Password : Entity
    {
        [NotNull]
        public string Hash { get; protected set; }

        [NotNull]
        public string Salt { get; protected set; }
        public PasswordStatus Status { get; protected set; }
        public DateTime CreatedAt { get; protected set; }
        public DateTime UpdatedAt { get; protected set; }
        public Account Account { get; protected set; }

        protected Password()
        {
        }

        public Password([NotNull] string hash, [NotNull] string salt)
        {
            Hash = hash;
            Salt = salt;
            Activate();
            CreatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Set first password after created account or change password for next time.
        /// </summary>
        /// <param name="hash">Password hash</param>
        public void SetHash(string hash)
        {
            if (String.IsNullOrWhiteSpace(hash))
                throw new Exception($"Argument {hash} can not be null or empty.");

            Hash = hash;
            UpdatedDate();
        }

        /// <summary>
        /// Sets the salt for current password.
        /// </summary>
        /// <param name="salt"></param>
        public void SetSalt(string salt)
        {
            if (String.IsNullOrWhiteSpace(salt))
                throw new Exception($"Argument {salt} can not be null or empty.");

            Salt = salt;
            UpdatedDate();
        }

        public void Activate()
        {
            Status = PasswordStatus.Active;
            UpdatedDate();
        }

        public void SetBlockade()
        {
            Status = PasswordStatus.Blocked;
            UpdatedDate();
        }

        private void UpdatedDate()
        {
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
