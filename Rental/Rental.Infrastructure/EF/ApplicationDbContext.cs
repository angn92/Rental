using Microsoft.EntityFrameworkCore;
using MimeKit;
using Rental.Core.Domain;
using Rental.Infrastructure.UpgradeDatabase;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Rental.Infrastructure.EF
{
    public class ApplicationDbContext : DbContext
    {
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Password> Passwords { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Session> Sessions { get; set; }
        public virtual DbSet<Dictionary> Dictionaries { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Session>()
                .HasKey(x => x.SessionIdentifier);

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

            //modelBuilder.Entity<Dictionary>()
            //    .HasOne<>
            modelBuilder.Entity<Dictionary>()
                    .Property<string>("Value")
                    .HasMaxLength(255);

            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries().Where(x => x.State == EntityState.Added || x.State == EntityState.Modified);

            foreach (var item in entries)
            {
                item.Property("UpdatedAt").CurrentValue = DateTime.UtcNow;
                
                if(item.State == EntityState.Added)
                {
                    item.Property("CreatedAt").CurrentValue = DateTime.UtcNow;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
