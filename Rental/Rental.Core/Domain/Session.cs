﻿using Rental.Core.Enum;
using System;

namespace Rental.Core.Domain
{
    public class Session : Entity
    {
        public string SessionId { get; protected set; }
        public string State { get; protected set; }
        public User UserIdentifier { get; protected set; }
        public DateTime LastAccessDate { get; protected set; }

        public Session(string sessionId, string state, User userSession)
        {
            SessionId = sessionId;
            State = state;
            UserIdentifier = userSession;
            UpdateLastAccessDate();
        }

        public void UpdateLastAccessDate()
        {
            LastAccessDate = DateTime.UtcNow;
        }

        public void SetSessionActive()
        {
            State = SessionState.NotActive.ToString();
        }
    }
}
