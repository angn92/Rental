using JetBrains.Annotations;
using Rental.Core.Base;
using Rental.Core.Enum;
using System;

namespace Rental.Core.Domain
{
    public class Product : Entity
    {
        public virtual string ProductId { get; protected set; }
        public virtual string Name { get; protected set; }
        public virtual int Amount { get; protected set; }
        public virtual int QuantityAvailable { get; protected set; }
        public virtual string Description { get; protected set; }
        public virtual ProductStatus Status { get; protected set; }
        public virtual Category Category { get; protected set; }
        public virtual Customer Customer { get; protected set; }

        protected Product()
        {}

        public Product([NotNull] string name, [NotNull] int amount, [NotNull] Category category)
        {
            ProductId = Guid.NewGuid().ToString();
            SetName(name);
            SetAmoutProducts(amount);
            QuantityAvailable = amount;
            SetAvailableStatus();
            Category = category;
        }

        public void SetName(string name)
        {
            Name = name;
        }

        public void SetAvailableStatus()
        {
            Status = ProductStatus.Available;
        }

        public void SetReservedStatus()
        {
            Status = ProductStatus.Reserved;
        }

        public void SetAmoutProducts(int amount)
        {
            Amount = amount;
        }
    }
}