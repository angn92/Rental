using System;

namespace Rental.Core.Domain
{
    public abstract class Entity
    {
        public DateTime UpdatedAt { get; protected set; }
        public DateTime CreatedAt { get; protected set; }
    }
}
