using JetBrains.Annotations;
using System;
using System.Collections.Generic;

namespace Rental.Core.Domain
{
    public class Category : Entity
    {
        public string Name { get; protected set; }
        public ISet<Product> Products { get; protected set; }
       
        protected Category()
        {
        }

        public Category([NotNull] string name)
        {
            Setname(name);
        }

        private void Setname(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new Exception($"Argument {name} is incorrect.");

            Name = name;
        }
    }
}