﻿using FluentAssertions;
using NUnit.Framework;
using Rental.Core.Enum;
using Rental.Infrastructure.Exceptions;
using Rental.Infrastructure.Helpers;
using Rental.Test.Helpers;
using System.Threading.Tasks;

namespace Rental.Test.UnitTest
{
    [TestFixture]
    public class ProductTest : TestBase
    {
        private ProductHelper _productHelper;

        [SetUp]
        public void SetUp()
        {
            _productHelper = new ProductHelper();
        }

        [Test]
        public async Task ShouldBeAbleAddNewProduct()
        {
            //ARRANGE
            var customer = CustomerTestHelper.CreateCustomer(_context, firstName, lastName, username, email);
            var category = CategoryTestHelper.CreateNewCategory(_context, "Electronics");

            //ACT
            await _productHelper.AddProductAsync(_context, "Iphone 14", 1, customer, category, "New Iphone model");

            //ASSERT
            var product = ProductTestHelper.GetCustomerProduct(_context, customer);
            product.Should().NotBeNull();
            product.Status.Should().Be(ProductStatus.Available);
            product.Customer.Should().Be(customer);
        }

        [Test]
        public void ShouldBeAbleSetProductStatus_Reserved()
        {
            //ARRANGE
            var customer = CustomerTestHelper.CreateCustomer(_context, firstName, lastName, username, email);
            var category = CategoryTestHelper.CreateNewCategory(_context, "Electronics");
            var product = ProductTestHelper.AddProduct(_context, "Iphone", 2, category, customer);

            //ACT
            _productHelper.MakeReservationProduct(product);

            //ASSERT
            product.Status.Should().Be(ProductStatus.Reserved);
            product.QuantityAvailable.Should().Be(1);
        }

        [Test]
        public async Task ShouldBeAbleGetProduct()
        {
            //ARRANGE
            var customer = CustomerTestHelper.CreateCustomer(_context, firstName, lastName, username, email);

            var category = CategoryTestHelper.CreateNewCategory(_context, "Sport");

            var existingProduct = ProductTestHelper.AddProduct(_context, "Bike", 1, category, customer);

            //ACT
            var product = await _productHelper.GetProductAsync(_context, existingProduct.ProductId);

            //ASSERT
            product.Should().NotBeNull();
            product.Status.Should().Be(ProductStatus.Available);
        }

        [Test]
        public async Task ShouldNotBeAbleGetProduct_ProductNotExist()
        {
            //ARRANGE
            var customer = CustomerTestHelper.CreateCustomer(_context, firstName, lastName, username, email);

            var category = CategoryTestHelper.CreateNewCategory(_context, "Sport");

            var existingProduct = ProductTestHelper.AddProduct(_context, "Bike", 1, category, customer);

            //ACT
            var exception = Assert.ThrowsAsync<CoreException>(() => _productHelper.GetProductAsync(_context, "wrongProductId"));

            //ASSERT
            exception.Code.Should().Be(ErrorCode.ProductNotExist);
        }
    }
}
