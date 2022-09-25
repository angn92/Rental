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
        public virtual DateTime? LastChangeDate { get; set; }
        public virtual DateTime? ExpirationDate { get; set; }
        public virtual DateTime? LastSuccessfulLoginDate { get; set; }
        public virtual DateTime? FailedLoginDate { get; set; }

        /// <summary>
        /// Password marker indicated that password is new. When is new user after correct first login will change status on Active. 
        /// Default password has status not active
        /// </summary>
        public virtual bool NewPassword { get; set; }

        public virtual int ActivationCode { get; set; }

        protected Password()
        {
        }

        public Password([NotNull] string hash, [NotNull] string salt, [NotNull] Customer customer, int code)
        {
            PasswordId = Guid.NewGuid().ToString();
            SetPasswordHash(hash);
            SetPasswordSalt(salt);
            SetPasswordNotActive();
            Customer = customer;
            NewPassword = true;
            ActivationCode = code;
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

        //After first correct log in password will change status on Active
        public void SetPasswordNotActive()
        {
            Status = PasswordStatus.NotActive;
        }

        public void ChangePasswordMarker()
        {
            NewPassword = false;
        }
    }
}
