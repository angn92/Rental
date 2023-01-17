using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;

namespace Rental.Api.CustomHeaders
{
    public class HeaderParametersFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "SessionId",
                @In = ParameterLocation.Header,
                Required = true,
                Schema = new OpenApiSchema
                {
                    Type = string.Empty
                }
                
            });
        }
    }
}
