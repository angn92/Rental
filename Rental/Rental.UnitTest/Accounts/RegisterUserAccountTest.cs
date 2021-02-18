using FluentAssertions;
using NUnit.Framework;
using Rental.UnitTest.TestHelpers;
using System;

namespace Rental.UnitTest.Accounts
{
    [TestFixture]
    public class RegisterUserAccountTest
    {
        string firstName = "Jan";
        string lastName = "Nowak";
        string username = "Jan00";
        string email = "jan@email.com";
        string phone = "123456789";
 
        [Test]
        public void ShouldBeAbleRegisterUserAccount()
        {
            var accountUser = UserTestHelper.CreateUser(firstName, lastName, username, email, phone);
            accountUser.Should().NotBeNull();
        }

        [Test]
        public void ShouldNotBeAbleRegisterUserAccountWhenFirstNameIsNotCorrect()
        {
            string firstName = "";
            Assert.Throws<Exception>(() => UserTestHelper.CreateUser(firstName, lastName, username, email, phone));
        }

        [Test]
        public void ShouldNotBeAbleRegisterUserAccountWhenLasttNameIsNotCorrect()
        {
            string lastName = "";
            Assert.Throws<Exception>(() => UserTestHelper.CreateUser(firstName, lastName, username, email, phone));
        }

        [Test]
        public void ShouldNotBeAbleRegisterUserAccountWhenUsernameIsNotCorrect()
        {
            string username = "";
            Assert.Throws<Exception>(() => UserTestHelper.CreateUser(firstName, lastName, username, email, phone));
        }

        [Test]
        public void ShouldNotBeAbleRegisterUserAccountWhenEmailIsNotCorrect()
        {
            string email = "";
            Assert.Throws<Exception>(() => UserTestHelper.CreateUser(firstName, lastName, username, email, phone));
        }
    }
}
