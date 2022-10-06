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
        /// <param name="sessionId"></param>
        /// <returns></returns>
        [HttpGet("detail/{id}")]
        public async Task<OrderDetailsRs> DisplayOrderDetails([FromRoute][NotNull] string id, [FromBody] int sessionId)
        {
            var query = new OrderDetailsRq
            {
                OrderId = id,
                SessionId = sessionId
            };

            return await queryDispatcher.DispatchAsync<OrderDetailsRq, OrderDetailsRs>(query);
        }

        /// <summary>
        /// Return all current active orders for given user
        /// </summary>
        /// <param name="username"></param>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        [HttpGet("activeOrders")]
        public async Task<ActiveOrdersRs> GetAllActiveOrders([FromBody][NotNull] string username, [NotNull] int sessionId)
        {
            var query = new ActiveOrdersRq
            {
                Username = username,
                SessionId = sessionId
            };

            return await queryDispatcher.DispatchAsync<ActiveOrdersRq, ActiveOrdersRs>(query);
        }
    }
}