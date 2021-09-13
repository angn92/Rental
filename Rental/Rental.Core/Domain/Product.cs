using JetBrains.Annotations;
using Rental.Core.Enum;
using System;

namespace Rental.Core.Domain
{
    public class Product : Entity
    {
        public virtual int ProductId { get; set; }
        public virtual string Name { get; set; }
        public virtual int Amount { get; set; }
        public virtual int QuantityAvailable { get; set; }
        public virtual string Description { get; set; }
        public virtual ProductStatus Status { get; set; }
        public virtual Category Category { get; set; }
        public virtual Customer User { get; set; }

        protected Product()
        {
        }

        public Product([NotNull] string name, [NotNull] int amount, [NotNull] Category category)
        {
            Id = Guid.NewGuid();
            Name = name;
            Amount = amount;
            QuantityAvailable = Amount;
            SetAvailableStatus();
            Category = category;
        }

        public void SetAvailableStatus()
        {
            Status = ProductStatus.Available;
        }

        public void SetReservedStatus()
        {
            Status = ProductStatus.Reserved;
        }
    }
}