using Microsoft.AspNetCore.Http;
using Rental.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rental.Test
{
    public class IntegrationTestHttp : IHttpContextAccessor
    {
        public HttpContext HttpContext 
        { 
            get
            {
                var httpContext = new DefaultHttpContext();
                httpContext.Request.Headers["SessionId"] = "111111";

                return httpContext;
            }
            set => throw new NotImplementedException(); 
        }
    }
}
