using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using Rental.Core.Domain;
using Rental.Infrastructure.EF;
using Rental.Infrastructure.Exceptions;
using Rental.Infrastructure.Helpers;
using Rental.Infrastructure.Services.UserService;
using System.Threading.Tasks;

namespace Rental.UnitTest.User
{
    [TestFixture]
    public class UserServicesTest
    {
        private DbContextOptions<ApplicationDbContext> _options;

        [SetUp]
        public void SetUp()
        {
            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDB")
                .Options;
        }

        [Test]
        public async Task ShouldBeAbleCheckThatGivenUsernameExist()
        {
            // Arrange
            var email = new Mock<IEmailValidator>();
            var password = new Mock<IPasswordHelper>();

            using (var context = new ApplicationDbContext(_options))
            {
                await context.Customers.AddAsync(new Customer("user1", "user", "nickname", "email@email.com", "123456789"));
                await context.SaveChangesAsync();
            }

            using (var context = new ApplicationDbContext(_options))
            {
                var userService = new UserService(context, email.Object, password.Object);

                var exist = await userService.CheckIfExist("nickname");

                Assert.IsTrue(exist);
            }
            
        }

        [Test]
        public async Task ShouldBeAbleGetCustomer()
        {
            // Arrange
            var email = new Mock<IEmailValidator>();
            var password = new Mock<IPasswordHelper>();

            using (var context = new ApplicationDbContext(_options))
            {
                await context.Customers.AddAsync(new Customer("user1", "user", "nickname", "email@email.com", "123456789"));
                await context.SaveChangesAsync();
            }

            using (var context = new ApplicationDbContext(_options))
            {
                var userService = new UserService(context, email.Object, password.Object);

                var user = await userService.GetCustomerAsync("nickname");

                Assert.IsNotNull(user);
                Assert.AreEqual("user1", user.FirstName);
            }
        }

        [Test]
        public async Task ShouldReturnErrorMessageWhenUserDoesNotExist()
        {
            // Arrange
            var email = new Mock<IEmailValidator>();
            var password = new Mock<IPasswordHelper>();

            using (var context = new ApplicationDbContext(_options))
            {
                await context.Customers.AddAsync(new Customer("user1", "user", "nickname", "email@email.com", "123456789"));
                await context.SaveChangesAsync();
            }

            using (var context = new ApplicationDbContext(_options))
            {
                var userService = new UserService(context, email.Object, password.Object);

                var a = Assert.ThrowsAsync<CoreException>(() => userService.GetCustomerAsync("Nickname1"));

                Assert.AreEqual(ErrorCode.UserNotExist, a.Code);
            }
        }
    }
}
