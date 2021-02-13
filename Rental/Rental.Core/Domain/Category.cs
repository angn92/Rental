using JetBrains.Annotations;
using System;
using System.Collections.Generic;

namespace Rental.Core.Domain
{
    public class Category : Entity
    {
        [NotNull]
        public string Name { get; protected set; }
        public ISet<Product> Products { get; protected set; }
        public DateTime CreatedAt { get; protected set; }
        public DateTime UpdatedAt { get; protected set; }

        protected Category()
        {
        }

        public Category([NotNull] string name)
        {
            Setname(name);
            CreatedAt = DateTime.UtcNow;
        }

        private void Setname(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new Exception($"Argument {name} is incorrect.");

            Name = name;
            UpdatedDate();
        }

        private void UpdatedDate()
        {
            UpdatedAt = DateTime.UtcNow;
        }
    }
}