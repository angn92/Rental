using Microsoft.EntityFrameworkCore;
using Rental.Core.Domain;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Rental.Infrastructure.EF
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Password> Passwords { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Session> Sessions { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Relationships for Customer entity
            modelBuilder.Entity<Customer>()
                    .HasMany<Password>(p => p.Passwords)
                    .WithOne(c => c.Customer);

            modelBuilder.Entity<Customer>()
                    .HasMany<Product>(p => p.Products)
                    .WithOne(c => c.Customer);

            modelBuilder.Entity<Customer>()
                    .HasOne<Session>(c => c.Session)
                    .WithOne(s => s.Customer)
                    .HasForeignKey<Session>(s => s.IdCustomer);

            //Relationships for Password entity
            modelBuilder.Entity<Password>()
                    .HasOne<Customer>(p => p.Customer)
                    .WithMany(c => c.Passwords);

            //Relationships for Category entity
            modelBuilder.Entity<Category>()
                    .HasMany<Product>(p => p.Products)
                    .WithOne(c => c.Category);

            //Relationships for Product entuty
            modelBuilder.Entity<Product>()
                    .HasOne<Category>(c => c.Category)
                    .WithMany(p => p.Products);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries().Where(x => x.State == EntityState.Added || x.State == EntityState.Modified);

            foreach (var item in entries)
            {
                item.Property("UpdatedAt").CurrentValue = DateTime.Now;
                
                if(item.State == EntityState.Added)
                {
                    item.Property("CreatedAt").CurrentValue = DateTime.Now;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
