using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using Rental.Core.Domain;
using Rental.Infrastructure.EF;
using Rental.Infrastructure.Exceptions;
using Rental.Infrastructure.Helpers;
using Rental.Infrastructure.Services.CustomerService;
using System;
using System.Threading.Tasks;

namespace Rental.UnitTest.User
{
    [TestFixture]
    public class UserServicesTest
    {
        private DbContextOptions<ApplicationDbContext> _options;
        private string _firstName, _lastName, _userName, _email, _phone, _password;

        [SetUp]
        public void SetUp()
        {
            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _firstName = "Jan";
            _lastName = "Nowak";
            _userName = "username";
            _email = "email@email.com";
            _phone = "123456789";
            _password = "pass";
        }

        [Test]
        public async Task ShouldBeAbleCheckThatGivenUsernameExist()
        {
            // Arrange
            var email = new Mock<IEmailHelper>();
            var password = new Mock<IPasswordHelper>();

            using (var context = new ApplicationDbContext(_options))
            {
                await context.Customers.AddAsync(new Customer(_firstName, _lastName, _userName, _email, _phone));
                await context.SaveChangesAsync();
            }

            using (var context = new ApplicationDbContext(_options))
            {
                var userService = new CustomerService(context, email.Object, password.Object);

                var exist = await userService.CheckIfExist(_userName);

                Assert.IsTrue(exist);
            }
        }

        [Test]
        public async Task ShouldBeAbleGetCustomer()
        {
            // Arrange
            var email = new Mock<IEmailHelper>();
            var password = new Mock<IPasswordHelper>();

            using (var context = new ApplicationDbContext(_options))
            {
                await context.Customers.AddAsync(new Customer(_firstName, _lastName, _userName, _email, _phone));
                await context.SaveChangesAsync();
            }

            using (var context = new ApplicationDbContext(_options))
            {
                var userService = new CustomerService(context, email.Object, password.Object);

                var user = await userService.GetCustomerAsync(_userName);

                Assert.IsNotNull(user);
                Assert.AreEqual("Jan", user.FirstName);
            }
        }

        [Test]
        public async Task ShouldReturnErrorMessageWhenUserDoesNotExist()
        {
            // Arrange
            var email = new Mock<IEmailHelper>();
            var password = new Mock<IPasswordHelper>();

            using (var context = new ApplicationDbContext(_options))
            {
                await context.Customers.AddAsync(new Customer(_firstName, _lastName, _userName, _email, _phone));
                await context.SaveChangesAsync();
            }

            using (var context = new ApplicationDbContext(_options))
            {
                var userService = new CustomerService(context, email.Object, password.Object);

                var exception = Assert.ThrowsAsync<CoreException>(() => userService.GetCustomerAsync("Nickname1"));

                Assert.AreEqual(ErrorCode.UserNotExist, exception.Code);
            }
        }

        [Test]
        public async Task ShouldBeAbleRegisterUser()
        {
            //Arrange
            var email = new Mock<IEmailHelper>();
            var password = new Mock<IPasswordHelper>();

            using (var context = new ApplicationDbContext(_options))
            {
                var userService = new CustomerService(context, email.Object, password.Object);
                await userService.RegisterAsync(_firstName, _lastName, _userName, _email, _phone);

                var registeredUser = context.Customers.FirstOrDefaultAsync(x => x.Username == _userName);

                Assert.NotNull(registeredUser);
            }
        }

        [Test]
        public async Task ShouldNotBeAbleRegisterUser_GivenUserAlreadyExist()
        {
            //Arrange
            var email = new Mock<IEmailHelper>();
            var password = new Mock<IPasswordHelper>();

            using (var context = new ApplicationDbContext(_options))
            {
                await context.Customers.AddAsync(new Customer(_firstName, _lastName, _userName, _email, _phone));
                await context.SaveChangesAsync();
            }

            using (var context = new ApplicationDbContext(_options))
            {
                var userService = new CustomerService(context, email.Object, password.Object);
                var exception = Assert.ThrowsAsync<CoreException>(() => userService.RegisterAsync(_firstName, _lastName, _userName, _email, _phone));

                Assert.AreEqual(ErrorCode.UsernameExist, exception.Code);
            }
        }
    }
}
