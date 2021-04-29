using System;

namespace Rental.Core.Domain
{
    public abstract class Entity
    {
        public Guid Id { get; protected set; }
        public DateTime UpdatedAt { get; protected set; }
        public DateTime CreatedAt { get; protected set; }

        public Entity()
        {
            Id = Guid.NewGuid();
        }
    }
}
