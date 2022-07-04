using JetBrains.Annotations;
using Rental.Core.Base;
using Rental.Core.Enum;
using System;

namespace Rental.Core.Domain
{
    public class Session : Entity
    {
        /// <summary>
        /// Session identifier
        /// </summary>
        public virtual int SessionId { get; set; }

        /// <summary>
        /// Session state 
        /// </summary>
        public virtual SessionState State { get; set; }

        /// <summary>
        /// Created date
        /// </summary>
        public virtual DateTime GenerationDate { get; set; }

        /// <summary>
        /// Last access session
        /// </summary>
        public virtual DateTime LastAccessDate { get; set; }

        /// <summary>
        /// Customer id 
        /// </summary>
        public virtual int IdCustomer { get; set; }

        /// <summary>
        /// Customer
        /// </summary>
        public virtual Customer Customer { get; set; }

        protected Session()
        {
        }

        public Session([NotNull] int sessionId, [NotNull] Customer customer)
        {
            SessionId = sessionId;
            State = SessionState.NotAuthorized;
            UpdateLastAccessDate();
            GenerationDate = DateTime.UtcNow;
            Customer = customer;
        }

        public void UpdateLastAccessDate()
        {
            LastAccessDate = DateTime.UtcNow;
        }

        public void SetSessionActive()
        {
            State = SessionState.Active;
        }

        public void SetSessionExpired()
        {
            State = SessionState.Expired;
        }
    }
}
