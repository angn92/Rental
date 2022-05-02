using Microsoft.AspNetCore.Http;

namespace RentalCommon.Headers
{
    public class ResponseMiddleware
    {
        private readonly RequestDelegate _next;

        public ResponseMiddleware(RequestDelegate requestDelegate)
        {
            _next = requestDelegate;
        }

        public Task Invoke(HttpContext context)
        {
            context.Response.Headers.Add("x-application-name", "Rental");

            context.Response.Headers.Add("x-request-id", "rqid");

            context.Response.Headers.Add("Authorization", "qwe");

            context.Response.Headers.Add("AcceptLanguage", "en-US");

            return _next(context);
        }

    }
}
