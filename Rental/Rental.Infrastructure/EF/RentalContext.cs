using Microsoft.EntityFrameworkCore;
using Rental.Core.Domain;

namespace Rental.Infrastructure.EF
{
    public class RentalContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Password> Passwords { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        public RentalContext(DbContextOptions<RentalContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Password>()
                    .HasOne<Account>(u => u.Account)
                    .WithMany(p => p.Passwords);

            modelBuilder.Entity<Account>()
                    .HasMany<Password>(u => u.Passwords)
                    .WithOne(p => p.Account);

            modelBuilder.Entity<Account>()
                    .HasMany<Product>(u => u.Products)
                    .WithOne(p => p.Account);

            modelBuilder.Entity<Category>()
                    .HasMany<Product>(c => c.Products)
                    .WithOne(p => p.Category);
        }
    }
}
