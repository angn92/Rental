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
            context.Response.Headers.Add("App-Name", "Rental");

            context.Response.Headers.Add("Request-Id", "rqid");

            context.Response.Headers.Add("App-Version", "qwe");

            context.Response.Headers.Add("AcceptLanguage", "en-US");
            

            return _next(context);
        }

    }
}
