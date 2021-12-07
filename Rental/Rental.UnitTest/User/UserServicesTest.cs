using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using Rental.Core.Domain;
using Rental.Infrastructure.EF;
using Rental.Infrastructure.Helpers;
using Rental.Infrastructure.Services.UserService;
using System.Threading.Tasks;

namespace Rental.UnitTest.User
{
    [TestFixture]
    public class UserServicesTest
    {
        [Test]
        public async Task ShouldBeAbleCheckThatGivenUserExist()
        {
            // Arrange
            var option = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDB")
                .Options;

            var email = new Mock<IEmailValidator>();
            var password = new Mock<IPasswordHelper>();

            using (var context = new ApplicationDbContext(option))
            {
                await context.Customers.AddAsync(new Customer("user1", "user", "nickname", "email@email.com", "123456789"));
                await context.SaveChangesAsync();
            }

            using (var context = new ApplicationDbContext(option))
            {
                var userService = new UserService(context, email.Object, password.Object);

                var exist = await userService.CheckIfExist("nickname");

                Assert.IsTrue(exist);
            }
            
        }
    }
}
