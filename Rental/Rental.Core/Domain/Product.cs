using JetBrains.Annotations;
using Rental.Core.Enum;
using System;

namespace Rental.Core.Domain
{
    public class Product : Entity
    {
        public string Name { get; protected set; }
        public int Amount { get; protected set; }
        public int QuantityAvailable { get; protected set; }
        public string Description { get; protected set; }
        public ProductStatus Status { get; protected set; }
        public Category Category { get; protected set; }
        public User User { get; protected set; }

        protected Product()
        {

        }

        public Product([NotNull] string name, [NotNull] int amount, [NotNull] Category category)
        {
            Id = Guid.NewGuid();
            SetName(name);
            Amount = amount;
            QuantityAvailable = Amount;
            SetAvailableStatus();
            Category = category;
        }

        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new Exception($"Argument {name} is incorrect.");

            Name = name;
        }

        public void SetDescription(string description)
        {
            if (string.IsNullOrEmpty(description))
                throw new Exception($"Argument {description} is incorrect.");

            Description = description;
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