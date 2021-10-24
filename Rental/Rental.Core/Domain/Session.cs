using JetBrains.Annotations;
using Rental.Core.Base;
using Rental.Core.Enum;
using System;

namespace Rental.Core.Domain
{
    public class Session : Entity
    {
        public virtual string SessionId { get; protected set; }
        public virtual SessionState State { get; protected set; }
        public virtual DateTime GenerationDate { get; protected set; }
        public virtual DateTime LastAccessDate { get; protected set; }
        public virtual Customer Customer { get; protected set; }
        public virtual int CustomerId { get; protected set; }

        protected Session()
        {
        }

        public Session([NotNull] string sessionId, [NotNull] Customer customer)
        {
            SessionId = sessionId;
            State = SessionState.NotAuthorized;
            Customer = customer;
            UpdateLastAccessDate();
            GenerationDate = DateTime.UtcNow;
        }

        public void UpdateLastAccessDate()
        {
            LastAccessDate = DateTime.UtcNow;
        }

        public void SetSessionActive()
        {
            State = SessionState.NotActive;
        }
    }
}
