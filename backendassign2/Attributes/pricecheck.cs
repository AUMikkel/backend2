using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace backendassign2.Attributes;

public class PriceValidationAttribute : Attribute, IModelValidator
{
    public string ErrorMessage { get; set; } = "Price cannot be negative";

    public IEnumerable<ModelValidationResult> Validate(ModelValidationContext context)
    {
        // Ensure the context model is of the type you're validating
        if (context.Model is decimal price)
        {
            if (price < 0)
            {
                // Return a validation result with an empty member name to apply to the whole object
                return new List<ModelValidationResult>
                {
                    new ModelValidationResult("", ErrorMessage)
                };
            }
        }
        // If no validation error, return an empty result set
        return Enumerable.Empty<ModelValidationResult>();
    }
}