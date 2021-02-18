using Rental.Core.Domain;

namespace Rental.UnitTest.TestHelpers
{
    public static class UserTestHelper
    {
        public static Account CreateUser(string firstName, string lastName, string username, string email, string phone)
        {
            var account = new Account(firstName, lastName, username, email, phone);
            return account;
        }
    }
}
