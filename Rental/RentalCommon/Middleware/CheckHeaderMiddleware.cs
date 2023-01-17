using Microsoft.AspNetCore.Http;

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
            else
                await _next(httpContext);
        }
    }
}
