using JetBrains.Annotations;
using Rental.Core.Enum;
using System;

namespace Rental.Core.Domain
{
    public class Password : Entity
    {
        public virtual int PasswordId { get; set; }
        public virtual string Hash { get; set; }
        public virtual string Salt { get; set; }
        public virtual PasswordStatus Status { get; set; }
        public virtual Customer Customer { get; set; }

        protected Password()
        {
        }

        public Password([NotNull] string hash, [NotNull] string salt, [NotNull] Customer customer)
        {
            Hash = hash;
            Salt = salt;
            Status = PasswordStatus.Active;
            Customer = customer;
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
