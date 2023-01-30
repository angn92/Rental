using Microsoft.AspNetCore.Http;
using Rental.Core.Base;

namespace RentalCommon.Middleware
{
    public class CheckHeaderMiddleware
    {
        public readonly RequestDelegate _next;

        public CheckHeaderMiddleware(RequestDelegate next) => _next = next;

        public async Task InvokeAsync(HttpContext httpContext)
        {
            
            if (!httpContext.Request.Headers.ContainsKey("SessionId") && !(httpContext.Request.Path.Value.Contains("Session/Create") ||
                httpContext.Request.Path.Value.Contains("Register/Customer")))
            {
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                return;
            }

            await _next(httpContext);
        }
    }
}
