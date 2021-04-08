using Microsoft.EntityFrameworkCore;
using Rental.Core.Domain;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Rental.Infrastructure.EF
{
    public class RentalContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Password> Passwords { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Session> Sessions { get; set; }

        public RentalContext(DbContextOptions<RentalContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Password>()
                    .HasOne<User>(u => u.User)
                    .WithMany(p => p.Passwords);

            modelBuilder.Entity<User>()
                    .HasMany<Password>(u => u.Passwords)
                    .WithOne(p => p.User);

            modelBuilder.Entity<User>()
                    .HasMany<Product>(u => u.Products)
                    .WithOne(p => p.User);

            modelBuilder.Entity<User>()
                    .HasOne<Session>(u => u.IdSession)
                    .WithOne(x => x.User)
                    .HasForeignKey<Session>(x => x.UserId);

            modelBuilder.Entity<Product>()
                    .HasOne<Category>(p => p.Category)
                    .WithMany(c => c.Products);
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
