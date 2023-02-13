using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using Rental.Core.Domain;
using Rental.Core.Enum;
using Rental.Infrastructure.EF;
using Rental.Infrastructure.Exceptions;
using Rental.Infrastructure.Helpers;
using Rental.Test.Helpers;
using System;
using System.Threading.Tasks;


namespace Rental.Test.UnitTest
{
    [TestFixture]
    public class CustomerTest
    {
        private string firstName, lastName, email, username;
        private CustomerHelper _customerHelper;
        private DbContextOptions<ApplicationDbContext> _options;
        private ApplicationDbContext _context;

        private Mock<IEmailHelper> _emailHelperMock;

        [SetUp]
        public void SetUp()
        {
            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(_options);

            firstName = "Jan";
            lastName = "Kowalski";
            email = "jankowalski@email.com";
            username = "jan_kowalski";

            _emailHelperMock = new Mock<IEmailHelper>();
            _customerHelper = new CustomerHelper(_context, _emailHelperMock.Object);
        }

        [Test]
        public void RegisterCustomer_ShouldReturnNewCreatedCustomer()
        {
            CustomerTestHelper.CreateCustomer(_context, firstName, lastName, username, email);

            var customerResult = _customerHelper.RegisterAsync(firstName, lastName, username, email).Result;

            customerResult.Should().NotBeNull();
        }

        [Test]
        public void CheckAccountStatusMethod_ReturnCustomerStatus_NotActive()
        {
            var customer = CustomerTestHelper.CreateCustomer(_context, firstName, lastName, username, email);
            var result = _customerHelper.CheckAccountStatus(customer);

            result.Should().NotBeNull();
            result.Should().Be(AccountStatus.NotActive.ToString());
        }

        [Test]
        public void CheckAccountStatusMethod_ReturnCustomerStatus_Active()
        {
            var customer = CustomerTestHelper.CreateCustomer(_context, firstName, lastName, username, email, x =>
            {
                x.Status = AccountStatus.Active;
            });

            var result = _customerHelper.CheckAccountStatus(customer);

            result.Should().NotBeNull();
            result.Should().Be(AccountStatus.Active.ToString());
        }

        [Test]
        public void CheckAccountStatusMethod_ReturnCustomerStatus_Blocked()
        {
            var customer = CustomerTestHelper.CreateCustomer(_context, firstName, lastName, username, email, x =>
            {
                x.Status = AccountStatus.Blocked;
            });

            var result = _customerHelper.CheckAccountStatus(customer);

            result.Should().NotBeNull();
            result.Should().Be(AccountStatus.Blocked.ToString());
        }

        [Test]
        public void CheckIfExist_Method_ShouldReturn_True()
        {
            var customer = CustomerTestHelper.CreateCustomer(_context, firstName, lastName, username, email);
            var customerExist = _customerHelper.CheckIfExist(customer.Username);

            customerExist.Should().BeTrue();
        }

        [Test]
        public void CheckIfExist_Method_ShouldReturn_False()
        {
            var customer = CustomerTestHelper.CreateCustomer(_context, firstName, lastName, username, email);

            var customerExist = _customerHelper.CheckIfExist("wrong_username");

            customerExist.Should().BeFalse();
        }

        [Test]
        public async Task ShouldBeAbleGetCustomer()
        {
            CustomerTestHelper.CreateCustomer(_context, firstName, lastName, username, email);

            var customer = await _customerHelper.GetCustomerAsync(username);

            customer.Should().NotBeNull();
            customer.Username.Should().Be(username);
        }

        [Test]
        public async Task ShouldReturnErrorMessageWhenUserDoesNotExist()
        {
            CustomerTestHelper.CreateCustomer(_context, firstName, lastName, username, email);
 
            var exception = Assert.ThrowsAsync<CoreException>(() => _customerHelper.GetCustomerAsync("wrong_username"));

            exception.Code.Should().Be(ErrorCode.UserNotExist);
        }

        [Test]
        public async Task ShouldNotBeAbleRegisterCustomer_InvalidInputData()
        {
            string FirstName = null;
            var exception = Assert.ThrowsAsync<Exception>(() => _customerHelper.RegisterAsync(FirstName, "Kowalski", "kowal123", "kowal@email.com"));

            exception.Message.Should().Be($"Registration is failed. Value cannot be null. (Parameter '{nameof(FirstName)}')");
            
        }

        [Test]
        public void ShouldBeAbleValidateCustomerAccount_AndThrowException_AccountIsBlocked()
        {
            var customer = CustomerTestHelper.CreateCustomer(_context, firstName, lastName, username, email, x =>
            {
                x.Status = AccountStatus.Blocked;
            });

            var exception = Assert.Throws<CoreException>(() => _customerHelper.ValidateCustomerAccount(customer));

            exception.Code.Should().Be(ErrorCode.AccountBlocked);
            exception.Message.Should().Be($"Account for customer {customer.Username} is blocked.");
        }

        [Test]
        public void ShouldBeAbleValidateCustomerAccount_AndThrowException_AccountIsNotActive()
        {
            var customer = CustomerTestHelper.CreateCustomer(_context, firstName, lastName, username, email, x =>
            {
                x.Status = AccountStatus.NotActive;
            });

            var exception = Assert.Throws<CoreException>(() => _customerHelper.ValidateCustomerAccount(customer));

            exception.Code.Should().Be(ErrorCode.AccountNotActive);
            exception.Message.Should().Be($"Account for customer {customer.Username} is not active.");
        }

        [Test]
        public void ShouldBeAbleChangeCustomerStatus()
        {
            var customer = CustomerTestHelper.CreateCustomer(_context, firstName, lastName, username, email, x =>
            {
                x.Status = AccountStatus.NotActive;
            });

            _customerHelper.ChangeAccountStatus(customer, AccountStatus.Active);

            customer = CustomerTestHelper.FindCustomer(_context, customer.Username);

            customer.Status.Should().Be(AccountStatus.Active);
        }
    }
}
