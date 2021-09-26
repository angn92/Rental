using JetBrains.Annotations;
using System;
using System.Collections.Generic;

namespace Rental.Core.Domain
{
    public class Category : Entity
    {
        public virtual string Name { get; set; }
        public virtual ISet<Product> Products { get; set; }
       
        protected Category()
        {
        }

        public Category([NotNull] string name)
        {
            Name = name;
        }
    }
}