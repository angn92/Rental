using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using Rental.Infrastructure.Helpers;
using Rental.Test.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Rental.Test.UnitTest
{
    [TestFixture]
    public class ProductTest : TestBase
    {
        [Test]
        public async Task ShouldBeAbleAddNewProduct()
        {
            // Arrange
            var productHelperMock = new Mock<IProductHelper>();
            var customer = CustomerTestHelper.CreateCustomer(_context, firstName, lastName, username, email, phone);
            var category = CategoryTestHelper.CreateNewCategory(_context, "Electronics");

            var productHelper = new ProductHelper();

            // Act
            await productHelper.AddProductAsync(_context, "Iphone", 1, customer, category, "New phone");

            // Assert
            var product = await _context.Products.FirstOrDefaultAsync(x => x.Name == "Iphone" && x.Customer.Username == customer.Username);
            Assert.IsNotNull(product);
        }

        [Test]
        public async Task ProductExist_ShouldNotBeAbleAddOnceAgain()
        {
            // Arrange
            var productHelperMock = new Mock<IProductHelper>();
            var customer = CustomerTestHelper.CreateCustomer(_context, firstName, lastName, username, email, phone);
            var category = CategoryTestHelper.CreateNewCategory(_context, "Electronics");


            var productHelper = new ProductHelper();
        }
    }
}
