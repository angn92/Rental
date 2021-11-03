using JetBrains.Annotations;
using Rental.Core.Base;
using Rental.Core.Enum;
using System;

namespace Rental.Core.Domain
{
    public class Session : Entity
    {
        public virtual int SessionId { get; protected set; }
        public virtual SessionState State { get; protected set; }
        public virtual DateTime GenerationDate { get; protected set; }
        public virtual DateTime LastAccessDate { get; protected set; }
        public virtual int IdCustomer { get; protected set; }
        public virtual Customer Customer { get; protected set; }

        protected Session()
        {
        }

        public Session([NotNull] int sessionId)
        {
            SessionId = sessionId;
            State = SessionState.NotAuthorized;
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
