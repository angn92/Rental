using JetBrains.Annotations;
using Rental.Core.Enum;
using System;

namespace Rental.Core.Domain
{
    public class Product : Entity
    {
        [NotNull]
        public string Name { get; protected set; }

        [NotNull]
        public int Amount { get; protected set; }

        [NotNull]
        public int QuantityAvailable { get; protected set; }

        [NotNull]
        public string Description { get; protected set; }
        public ProductStatus Status { get; protected set; }
        public Category Category { get; protected set; }
        public User User { get; protected set; }
        public DateTime CreatedAt { get; protected set; }
        public DateTime UpdatedAt { get; protected set; }

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
            CreatedAt = DateTime.UtcNow;
            UpdatedDate();
        }

        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new Exception($"Argument {name} is incorrect.");

            Name = name;
            UpdatedDate();
        }

        public void SetDescription(string description)
        {
            if (string.IsNullOrEmpty(description))
                throw new Exception($"Argument {description} is incorrect.");

            Description = description;
            UpdatedDate();
        }

        public void SetAvailableStatus()
        {
            Status = ProductStatus.Available;
            UpdatedDate();
        }

        public void SetReservedStatus()
        {
            Status = ProductStatus.Reserved;
            UpdatedDate();
        }

        private void UpdatedDate()
        {
            UpdatedAt = DateTime.UtcNow;
        }
    }
}