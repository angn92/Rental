using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using Rental.Core.Enum;
using Rental.Infrastructure.Exceptions;
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
            var productHelperMock = new Mock<ProductHelper>();
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
        public async Task VerifyThatGivenProductExist()
        {
            // Arrange
            var productHelperMock = new Mock<ProductHelper>();
            var customer = CustomerTestHelper.CreateCustomer(_context, firstName, lastName, username, email, phone);
            var category = CategoryTestHelper.CreateNewCategory(_context, "Electronics");
            var existingProduct = ProductTestHelper.AddProduct(_context, "Iphone", 1, category, customer);

            var productHelper = new ProductHelper();

            // Act
            var exception = Assert.ThrowsAsync<CoreException>(() => productHelper.CheckIfGivenProductExistAsync(_context, "Iphone", customer));

            // Assert
            Assert.AreEqual(ErrorCode.ProductExist, exception.Code);
        }

        [Test]
        public void ShouldBeAbleChangeProductStatusAfterMadeReservation()
        {
            // Arrange
            var productHelperMock = new Mock<ProductHelper>();
            var customer = CustomerTestHelper.CreateCustomer(_context, firstName, lastName, username, email, phone);
            var category = CategoryTestHelper.CreateNewCategory(_context, "Electronics");
            var existingProduct = ProductTestHelper.AddProduct(_context, "Iphone", 1, category, customer);

            var productHelper = new ProductHelper();
            
            // Act
            productHelper.MakeReservationProduct(existingProduct);

            // Assert
            Assert.AreEqual(ProductStatus.Reserved, existingProduct.Status);
        }

        [Test]
        public async Task ShouldBeAbleGetProduct()
        {
            // Arrange
            var productHelperMock = new Mock<ProductHelper>();
            var customer = CustomerTestHelper.CreateCustomer(_context, firstName, lastName, username, email, phone);
            var category = CategoryTestHelper.CreateNewCategory(_context, "Electronics");
            var existingProduct = ProductTestHelper.AddProduct(_context, "Iphone", 1, category, customer);

            var productHelper = new ProductHelper();

            // Act
            var product = await productHelper.GetProductAsync(_context, existingProduct.ProductId);

            // Assert
            Assert.IsNotNull(product);
            Assert.AreEqual(product.Name, existingProduct.Name);
        }
    }
}
