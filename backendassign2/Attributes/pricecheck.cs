using backendassign2.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace backendassign2.Attributes;

public class PriceValidationAttribute : Attribute, IModelValidator
{
    public string errorMessagePrice { get; set; } = "Price cannot be negative";
    public IEnumerable<ModelValidationResult> Validate(ModelValidationContext context)
    {
        if (context.Model is decimal price)
        {
            if (price < 0)
            {
                return new List<ModelValidationResult>
                {
                    new ModelValidationResult("", errorMessagePrice)
                };
            }
        }
        return Enumerable.Empty<ModelValidationResult>();
        
    }
}