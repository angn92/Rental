using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Rental.Infrastructure.Handlers.Utils.Query;
using Rental.Infrastructure.Query;
using System.Threading.Tasks;

namespace Rental.Api.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class ToolsController : Controller
    {
        private readonly IQueryDispatcher queryDispatcher;

        public ToolsController(IQueryDispatcher queryDispatcher)
        {
            this.queryDispatcher = queryDispatcher;
        }

        /// <summary>
        /// Endpoint to calculate hash for password and salt - only for test 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost("Generate/Hash")]
        public async Task<PasswordHashResponse> PrepareHashForPassword([NotNull] PasswordHashQuery query)
        {
            return await queryDispatcher.DispatchAsync<PasswordHashQuery, PasswordHashResponse>(query);
        }
         
    }
}
