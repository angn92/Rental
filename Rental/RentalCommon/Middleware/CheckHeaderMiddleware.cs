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
            if (!httpContext.Request.Headers.ContainsKey("SessionId"))
                httpContext.Response.StatusCode = 400;
            
            await _next(httpContext);
        }
    }
}
