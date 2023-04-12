using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Rental.Infrastructure.Handlers.Orders.Query.ActiveOrders;
using Rental.Infrastructure.Handlers.Orders.Query.OrderDetails;
using Rental.Infrastructure.Query;
using System.Threading.Tasks;

namespace Rental.Api.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class OrderController : Controller
    {
        private readonly IQueryDispatcher queryDispatcher;

        public OrderController(IQueryDispatcher queryDispatcher)
        {
            this.queryDispatcher = queryDispatcher;
        }

        /// <summary>
        /// Get full information about order 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("detail/{id}")]
        public async Task<OrderDetailsResponse> DisplayOrderDetails([FromRoute][NotNull] string id)
        {
            var query = new OrderDetailsRequest
            {
                OrderId = id
            };

            return await queryDispatcher.DispatchAsync<OrderDetailsRequest, OrderDetailsResponse>(query);
        }

        /// <summary>
        /// Return all current active orders for given user
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpGet("activeOrders/{username}")]
        public async Task<ActiveOrdersResponse> GetAllActiveOrders([FromRoute][NotNull] string username)
        {
            var query = new ActiveOrdersRequest
            {
                Username = username
            };

            return await queryDispatcher.DispatchAsync<ActiveOrdersRequest, ActiveOrdersResponse>(query);
        }
    }
}