﻿using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using Rental.Core.Domain;
using Rental.Core.Enum;
using Rental.Infrastructure.EF;
using Rental.Infrastructure.Exceptions;
using Rental.Infrastructure.Helpers;
using Rental.Infrastructure.Services.CustomerService;
using Rental.Test.Helpers;
using System;
using System.Threading.Tasks;

namespace Rental.Test.UnitTest
{
    [TestFixture]
    public class CustomerServicesTest
    {
        private DbContextOptions<ApplicationDbContext> _options;
        private ApplicationDbContext _context;

        [SetUp]
        public void SetUp()
        {
            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(_options);
        }

        [Test]
        public async Task ShouldBeAbleCheckThatGivenUsernameExist()
        {
            // Arrange
            var email = new Mock<IEmailHelper>();
            var password = new Mock<IPasswordHelper>();

            await _context.Customers.AddAsync(new Customer("firstName", "lastName", "username", "test@email.com", "123123123"));
            await _context.SaveChangesAsync();

            var userService = new CustomerService(_context, email.Object, password.Object);

            // Act
            var exist = await userService.CheckIfExist("username");

            // Assert
            Assert.IsTrue(exist);
        }

        [Test]
        public async Task ShouldBeAbleGetCustomer()
        {
            // Arrange
            var email = new Mock<IEmailHelper>();
            var password = new Mock<IPasswordHelper>();

            await _context.Customers.AddAsync(new Customer("firstName", "lastName", "username", "test@email.com", null));
            await _context.SaveChangesAsync();

            var userService = new CustomerService(_context, email.Object, password.Object);

            // Act
            var user = await userService.GetCustomerAsync("username");

            // Assert
            Assert.IsNotNull(user);
            Assert.AreEqual("username", user.FirstName);
            Assert.AreEqual(AccountStatus.Active.ToString(), user.Status.ToString());
            Assert.IsNull(user.Phone);
        }

        [Test]
        public async Task ShouldReturnErrorMessageWhenUserDoesNotExist()
        {
            // Arrange
            var email = new Mock<IEmailHelper>();
            var password = new Mock<IPasswordHelper>();

            await _context.Customers.AddAsync(new Customer("firstName", "lastName", "username", "test@email.com", "123123123"));
            await _context.SaveChangesAsync();

            var userService = new CustomerService(_context, email.Object, password.Object);

            // Act
            var exception = Assert.ThrowsAsync<CoreException>(() => userService.GetCustomerAsync("Nickname1"));

            // Assert
            Assert.AreEqual(ErrorCode.UserNotExist, exception.Code);
        }

        [Test]
        public async Task ShouldBeAbleRegisterUser()
        {
            //Arrange
            var email = new Mock<IEmailHelper>();
            var password = new Mock<IPasswordHelper>();
            var userService = new CustomerService(_context, email.Object, password.Object);

            // Act
            await userService.RegisterAsync("firstName", "lastName", "username", "test@email.com", "123123123");

            var registeredUser = await _context.Customers.FirstOrDefaultAsync(x => x.Username == "firstName");

            // Assert 
            Assert.NotNull(registeredUser);
            Assert.AreEqual("username", registeredUser.Username);
        }

        [Test]
        public void ShouldNotBeAbleRegisterUser_InvaalidEmail()
        {
            //Arrange
            var email = new Mock<IEmailHelper>();
            var password = new Mock<IPasswordHelper>();
            var userService = new CustomerService(_context, email.Object, password.Object);

            // Act
            var exception = Assert.ThrowsAsync<CoreException>(() => userService.RegisterAsync("firstName", "lastName", "username", null, "123123123"));

            // Assert
            Assert.AreEqual(ErrorCode.InvalidEmail, exception.Code);
        }

        [Test]
        public void ShouldBeAbleValidateCustomerAccount_AndThrowException_AccountIsBlocked()
        {
            // Arrange
            var email = new Mock<IEmailHelper>();
            var password = new Mock<IPasswordHelper>();

            var customer = CreateCustomerTestHelper.CreateCustomer(_context, "Jan", "Kowalski", "janek00", "jan@email.com", null, x =>
            {
                x.Status = AccountStatus.Blocked;
            });

            var userService = new CustomerService(_context, email.Object, password.Object);

            // Act
            var result = Assert.Throws<CoreException>(() => userService.ValidateCustomerAccount(customer));

            // Assert
            Assert.AreEqual(ErrorCode.AccountBlocked, result.Code);
            Assert.AreEqual("Account is blocked.", result.Message);
        }

        [Test]
        public void ShouldBeAbleValidateCustomerAccount_AndThrowException_AccountIsNotActive()
        {
            // Arrange
            var email = new Mock<IEmailHelper>();
            var password = new Mock<IPasswordHelper>();

            var customer = CreateCustomerTestHelper.CreateCustomer(_context, "Jan", "Kowalski", "janek00", "jan@email.com", null, x =>
            {
                x.Status = AccountStatus.NotActive;
            });

            var userService = new CustomerService(_context, email.Object, password.Object);

            // Act
            var result = Assert.Throws<CoreException>(() => userService.ValidateCustomerAccount(customer));

            // Assert
            Assert.AreEqual(ErrorCode.AccountNotActive, result.Code);
            Assert.AreEqual("Account is not active.", result.Message);
        }
    }
}
