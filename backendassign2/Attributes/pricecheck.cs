using backendassign2.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

public class PriceValidationAttribute : Attribute, IModelValidator
{
    public string errorMessage { get; set; } = "Price cannot be negative";
    public IEnumerable<ModelValidationResult> Validate(ModelValidationContext context)
    {
        var model = context.Model as ServiceDto.AddMealDto;
        if (model != null)
        {
            if (model.Price < 0)
            {
                return new List<ModelValidationResult>
                {
                    new ModelValidationResult("", errorMessage)
                };
            }
        }
        return Enumerable.Empty<ModelValidationResult>();
    }
}