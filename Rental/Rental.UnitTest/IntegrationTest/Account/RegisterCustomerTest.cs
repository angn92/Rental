using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Rental.Infrastructure.Command;
using Rental.Infrastructure.Exceptions;
using Rental.Infrastructure.Handlers.Account.Command.CreateAccount;
using Rental.Test.Helpers;
using System;

namespace Rental.Test.IntegrationTest.Account
{
    [TestFixture]
    public class RegisterCustomerTest : WebApplicationFactoryBase
    {
        [Test]
        public void ShouldBeAbleRegisterCustomer()
        {
            //ARRANGE
            var command = new RegisterCustomer
            {
                FirstName = "Shane",
                LastName = "Andersen",
                Username = "shane_andersen",
                Email = "shane_andersen@email.com",
                Password = "fake_password123"
            };

            //ACT
            var result = _factory.Services.GetRequiredService<ICommandHandler<RegisterCustomer, RegisterCustomerResponse>>().HandleAsync(command).Result;

            //ASSERT
            result.SessionId.Should().NotBeNull();
            var customer = CustomerTestHelper.FindCustomer(_context, command.Username);
            customer.Should().NotBeNull();
        }

        [Test]
        public void ShouldNotBeAbleRegisterCustomer_GivenUsernameExist()
        {
            //ARRANGE
            var customer = CustomerTestHelper.CreateCustomer(_context, "Shane", "Andersen", "shane_andersen", "shane_andersen@email.com");

            var command = new RegisterCustomer
            {
                FirstName = "Shane",
                LastName = "Andersen",
                Username = "shane_andersen",
                Email = "shane_andersen@email.com",
                Password = "fake_password123"
            };

            //ACT
            try
            {
                var exception = _factory.Services.GetRequiredService<ICommandHandler<RegisterCustomer, RegisterCustomerResponse>>().HandleAsync(command).Result;
            }
            catch(Exception ex)
            {
                ex.Message.Should().Be($"This username {command.Username} is in use.");
            }

            //ASSERT
            customer.Should().NotBeNull();
        }

        [Test]
        public void ShouldNotBeAbleRegisterCustomer_InvalidEmail()
        {
            //ARRANGE
            var command = new RegisterCustomer
            {
                FirstName = "Shane",
                LastName = "Andersen",
                Username = "shane_andersen",
                Email = "shane_andersen@email",
                Password = "fake_password123"
            };

            //ACT
            try
            {
                var exception = _factory.Services.GetRequiredService<ICommandHandler<RegisterCustomer, RegisterCustomerResponse>>().HandleAsync(command).Result;
            }
            catch (Exception ex)
            {
                ex.Message.Should().Be($"Registration is failed. Address email {command.Email} is incorrect");
            }
        }

        [Test]
        public void ShouldNotBeAbleRegisterCustomer_GivenEmailIsInUse()
        {
            //ARRANGE
            var customer = CustomerTestHelper.CreateCustomer(_context, "Shane", "Andersen", "shane_andersen", "shane_andersen@email.com");

            var command = new RegisterCustomer
            {
                FirstName = "Shane1",
                LastName = "Andersen",
                Username = "shane_andersen1",
                Email = "shane_andersen@email.com",
                Password = "fake_password123"
            };

            //ACT
            try
            {
                var exception = _factory.Services.GetRequiredService<ICommandHandler<RegisterCustomer, RegisterCustomerResponse>>().HandleAsync(command).Result;
            }
            catch (Exception ex)
            {
                ex.Message.Should().Be($"Registration is failed. Given address email {command.Email} is in use.");
            }

            //ASSERT
            var registeredCustomer = CustomerTestHelper.FindCustomer(_context, command.Username);
            registeredCustomer.Should().BeNull();
        }
    }
}
