using JetBrains.Annotations;
using Rental.Core.Base;
using Rental.Core.Enum;
using System;

namespace Rental.Core.Domain
{
    public class Password : Entity
    {
        public virtual string PasswordId { get; protected set; }
        public virtual string Hash { get; protected set; }
        public virtual string Salt { get; protected set; }
        public virtual PasswordStatus Status { get; protected set; }
        public virtual Customer Customer { get; protected set; }

        protected Password()
        {
        }

        public Password([NotNull] string hash, [NotNull] string salt, [NotNull] Customer customer)
        {
            PasswordId = Guid.NewGuid().ToString();
            SetPasswordHash(hash);
            SetPasswordSalt(salt);
            ActivatePassword();
            Customer = customer;
        }

        public void SetPasswordHash(string hash)
        {
            Hash = hash;
        }

        public void SetPasswordSalt(string salt)
        {
            Salt = salt;
        }

        public void ActivatePassword()
        {
            Status = PasswordStatus.Active;
        }

        public void BlockadePassword()
        {
            Status = PasswordStatus.Blocked;
        }
    }
}
