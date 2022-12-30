using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Rental.Infrastructure.EF;
using System;

namespace Rental.Test
{
    public abstract class TestBase
    {
        public DbContextOptions<ApplicationDbContext> _options;
        public ApplicationDbContext _context;
        


        public string firstName, lastName, username, email, phone;

        [SetUp]
        public void SetUp()
        {
            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(_options);

            firstName = "Jan";
            lastName = "Kowalski";
            email = "email@email.com";
            username = "jan_kowal";
            phone = "123456789";
        }
    }
}
