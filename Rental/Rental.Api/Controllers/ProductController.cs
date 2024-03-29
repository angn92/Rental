﻿using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Rental.Infrastructure.Command;
using Rental.Infrastructure.Handlers.Product.Command.BookingProduct;
using Rental.Infrastructure.Handlers.Product.Command.Cancel;
using Rental.Infrastructure.Handlers.Product.Command.NewProduct;
using Rental.Infrastructure.Handlers.Product.Query.ProductDetails;
using Rental.Infrastructure.Query;
using System.Threading.Tasks;

namespace Rental.Api.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly ICommandDispatcher commandDispacher;
        private readonly IQueryDispatcher queryDispatcher;

        public ProductController(ICommandDispatcher commandDispacher, IQueryDispatcher queryDispatcher)
        {
            this.commandDispacher = commandDispacher;
            this.queryDispatcher = queryDispatcher;
        }

        /// <summary>
        /// Add new product for customer to rental.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        public async Task AddProductAsync([FromBody] [NotNull] ProductRequest request)
        {
            var command = new ProductRequest
            {
                Name = request.Name,
                Amount = request.Amount,
                CategoryName = request.CategoryName,
                Description = request.Description,
                Username = request.Username
            };

            await commandDispacher.DispatchAsync(new ProductCommand(command));
        }

        /// <summary>
        /// Get product details.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{Id}")]
        public async Task<ProductDetailsResponse> GetProductDetailsAsync([FromRoute] [NotNull] string id)
        {
            var request = new ProductDetailRequest
            {
                ProductId = id
            };

            return await queryDispatcher.DispatchAsync<ProductDetailRequest, ProductDetailsResponse>(request);
        }

        /// <summary>
        /// Make a reservation product
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("Booking")]
        public async Task<ProductBookingResponse> BookingProductAsync([FromBody] ProductBookingRequest request)
        {
            var command = new ProductBookingCommand(request);

            return await commandDispacher.DispatchAsync<ProductBookingCommand, ProductBookingResponse>(command);
        }

        /// <summary>
        /// Cancel reservation for the borrowed product.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("Cancel")]
        public async Task CancelReservation([FromBody] CancelReservationCommand command)
        {
            await commandDispacher.DispatchAsync<CancelReservationCommand>(command);
        }
    }
}
