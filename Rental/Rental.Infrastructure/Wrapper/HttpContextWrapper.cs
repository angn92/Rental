using Microsoft.AspNetCore.Http;
using System.Linq;

namespace Rental.Infrastructure.Wrapper
{
    public interface IHttpContextWrapper
    {
        string GetValueFromRequestHeader(string key);
    }

    public class HttpContextWrapper : IHttpContextWrapper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpContextWrapper()
        {
        }

        public HttpContextWrapper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetValueFromRequestHeader(string key)
        {
            if(_httpContextAccessor.HttpContext.Request.Headers.TryGetValue(key, out var value) && value.Any())
            {
                return value.First();
            }

            return default;
        }
    }
}
