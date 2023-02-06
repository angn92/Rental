using Microsoft.EntityFrameworkCore;
using Rental.Core.Domain;
using System;

namespace Rental.Infrastructure.UpgradeDatabase
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Category> builder)
        {
            builder.HasData(new Category("Sprzęt budowlany"));
            builder.HasData(new Category("Sprzęt elektroniczny"));
            builder.HasData(new Category("Sport i rekreacja"));
            builder.HasData(new Category("Sprzęt domowy"));
        }
    }
}
