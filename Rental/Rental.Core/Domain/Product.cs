using JetBrains.Annotations;
using Rental.Core.Base;
using Rental.Core.Enum;
using System;

namespace Rental.Core.Domain
{
    public class Product : Entity
    {
        public virtual string ProductId { get; set; }
        public virtual string Name { get; set; }
        public virtual int Amount { get; set; }
        public virtual int QuantityAvailable { get; set; }
        public virtual string Description { get; set; }
        public virtual ProductStatus Status { get; set; }
        public virtual Category Category { get; set; }
        public virtual Customer Customer { get; set; }

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