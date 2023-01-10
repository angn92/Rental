using Microsoft.AspNetCore.Http;

namespace RentalCommon.Headers
{
    public class HeaderMiddleware
    {
        private readonly RequestDelegate _next;

        public HeaderMiddleware(RequestDelegate requestDelegate)
        {
            _next = requestDelegate;
        }

        public Task InvokeAsync(HttpContext context)
        {
            context.Response.Headers.Add("X-AppName", "Rental");
            context.Response.Headers.Add("X-TraceId", GetTraceId(context));
            context.Response.Headers.Add("X-AppVersion", "1.0.0");
            context.Response.Headers.Add("X-AcceptLanguage", "en-US");

            return _next(context);
        }

        public string GetTraceId(HttpContext context)
        {
            context.TraceIdentifier = Guid.NewGuid().ToString();
            return context.TraceIdentifier;
        }
    }
}
