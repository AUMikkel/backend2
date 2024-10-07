using backendassign2.Attributes;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace backendassign2.Swashbuckle;

public class PriceValidationSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        // Find the Price property that has PriceValidationAttribute
        var priceProperty = context.Type.GetProperties()
            .FirstOrDefault(p => p.CustomAttributes.Any(a => a.AttributeType == typeof(PriceValidationAttribute)));

        if (priceProperty != null)
        {
            if (schema.Properties.ContainsKey("price"))
            {
                var priceSchema = schema.Properties["price"];
                priceSchema.Description += " (Custom Validation: Price cannot be negative)";
            }
        }
    }
}