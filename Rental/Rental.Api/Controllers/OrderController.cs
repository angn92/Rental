using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
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
        [HttpGet("details/{id}")]
        public async Task<OrderDetailsRs> DisplayOrderDetails([FromRoute] [NotNull] string id, [FromBody] int sessionId)
        {
            var request = new OrderDetailsRq
            {
                OrderId = id,
                SessionId = sessionId
            };

            return await queryDispatcher.DispatchAsync<OrderDetailsRq, OrderDetailsRs>(request);
        }
    }
}
