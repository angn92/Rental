using Microsoft.EntityFrameworkCore;
using Rental.Infrastructure.EF;
using System;

namespace Rental.Test
{
    public class BaseServiceTest
    {
        public DbContextOptions<ApplicationDbContext> Options;

        public BaseServiceTest()
        {
            Options = new DbContextOptionsBuilder<ApplicationDbContext>()
                            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                            .Options;
        }

        public ApplicationDbContext GetContext() => new(Options);
    }
}
