using JetBrains.Annotations;
using Rental.Core.Enum;
using System;

namespace Rental.Core.Domain
{
    public class Password : Entity
    {
        public string Hash { get; protected set; }

        public string Salt { get; protected set; }

        public PasswordStatus Status { get; protected set; }
        public User User { get; protected set; }

        protected Password()
        {
        }

        public Password([NotNull] string hash, [NotNull] string salt, [NotNull] User user)
        {
            Hash = hash;
            Salt = salt;
            Activate();
            User = user;
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
        }

        public void Activate()
        {
            Status = PasswordStatus.Active;
        }

        public void SetBlockade()
        {
            Status = PasswordStatus.Blocked;
        }
    }
}
