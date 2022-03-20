using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Rental.Infrastructure.EF;

namespace Rental.Test
{
    public class TestBase
    {
        public DbContextOptions<ApplicationDbContext> _context;
        public string firstName, lastName, userName, email, password;

        [SetUp]
        public void SetContext()
        {
            _context = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(databaseName: "testDB")
                        .Options;
        }

        public ApplicationDbContext GetContext() => new(_context);

        [SetUp]
        public void SetBaseDataUser()
        {
            firstName = "Adam";
            lastName = "Nowak";
            userName = "adam123";
            email = "adam@email.com";
            password = null;
        }

        //[SetUp]
        //public void SetServices()
        //{

        //}
    }
}
