using System;

namespace Rental.Core.Base
{
    public abstract class Entity
    {
        public DateTime UpdatedAt { get; protected set; }
        public DateTime CreatedAt { get; protected set; }
    }
}
