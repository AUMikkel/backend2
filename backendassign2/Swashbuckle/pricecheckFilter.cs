using backendassign2.Attributes;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace backendassign2.Swashbuckle;

public class PriceValidationSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        // Find the Price property that has PriceValidationAttribute
        var priceProperty = context.MemberInfo?.CustomAttributes.Where(p => 
            p.AttributeType.Name == nameof(PriceValidationAttribute))
            .FirstOrDefault();

        if (priceProperty is not null)
        {
            //schema.Extensions.Add("ValidPrice", new OpenApiBoolean(true));
            schema.Extensions.Add("PriceCheck", new OpenApiString("Price can't be negative"));
        }
    }
}