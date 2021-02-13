using Microsoft.EntityFrameworkCore;
using Rental.Core.Domain;

namespace Rental.Infrastructure.EF
{
    public class RentalContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        //public DbSet<Password> Passwords { get; set; }
        //public DbSet<Category> Categories { get; set; }
        //public DbSet<Product> Products { get; set; }

        public RentalContext(DbContextOptions<RentalContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Password>()
            //        .HasOne<User>(u => u.User)
            //        .WithMany(p => p.Passwords);

            //modelBuilder.Entity<User>()
            //        .HasMany<Password>(u => u.Passwords)
            //        .WithOne(p => p.User);

            //modelBuilder.Entity<User>()
            //        .HasMany<Product>(u => u.Products)
            //        .WithOne(p => p.User);

            //modelBuilder.Entity<Category>()
            //        .HasMany<Product>(c => c.Products)
            //        .WithOne(p => p.Category);
        }
    }
}
