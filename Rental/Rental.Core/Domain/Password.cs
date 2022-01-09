using JetBrains.Annotations;
using Rental.Core.Base;
using Rental.Core.Enum;
using System;

namespace Rental.Core.Domain
{
    public class Password : Entity
    {
        public virtual string PasswordId { get; set; }
        public virtual string Hash { get; set; }
        public virtual string Salt { get; set; }
        public virtual PasswordStatus Status { get; set; }
        public virtual Customer Customer { get; set; }

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
