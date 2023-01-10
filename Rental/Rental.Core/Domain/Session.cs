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
        public virtual string SessionIdentifier { get; set; }

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

        public Session([NotNull] string sessionId, [NotNull] Customer customer, [NotNull] SessionState sessionState)
        {
            SessionIdentifier = sessionId;
            State = sessionState;
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

        public void ChangeState(SessionState sessionState)
        {
            State = sessionState;
        }
    }
}
