using Microsoft.AspNetCore.Http;

namespace RentalCommon.Headers
{
    public class HeaderMiddleware
    {
        private readonly RequestDelegate _next;

        public HeaderMiddleware(RequestDelegate requestDelegate) => _next = requestDelegate;

        public Task InvokeAsync(HttpContext httpContext)
        {
            httpContext.Response.Headers.Add("X-AppName", "Rental");
            httpContext.Response.Headers.Add("X-TraceId", GetTraceId(httpContext));
            httpContext.Response.Headers.Add("X-AppVersion", "1.0.0");
            httpContext.Response.Headers.Add("X-AcceptLanguage", "en-US");

            return _next(httpContext);
        }

        public string GetTraceId(HttpContext httpContext)
        {
            httpContext.TraceIdentifier = Guid.NewGuid().ToString();
            return httpContext.TraceIdentifier;
        }
    }
}
