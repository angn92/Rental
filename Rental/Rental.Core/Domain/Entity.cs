using System;

namespace Rental.Core.Domain
{
    public abstract class Entity
    {
        public Guid Id { get; protected set; }
        public Entity()
        {
            Id = Guid.NewGuid();
        }
    }
}
