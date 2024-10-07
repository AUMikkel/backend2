using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;

public class PriceValidationSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        // Check if the model contains the PriceValidationAttribute
        var priceProperty = context.Type.GetProperties()
            .FirstOrDefault(p => p.CustomAttributes
                .Any(a => a.AttributeType == typeof(PriceValidationAttribute)));
        
        if (priceProperty != null)
        {
            // Add a description in Swagger for the price property
            if (schema.Properties.ContainsKey(priceProperty.Name))
            {
                var priceSchema = schema.Properties[priceProperty.Name];
                
                // Add custom validation message and constraints
                priceSchema.Description += " (Custom Validation: Price cannot be negative)";
            }
        }
    }
}
