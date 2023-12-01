using System.ComponentModel.DataAnnotations;
using Blog.API.Data;

namespace Blog.API.Controllers.Dto.CustomValidationAttributes;

public class AddressAttribute: ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var context = (validationContext.GetService(typeof(AppDbContext)) as AppDbContext)!;

        if (value is Guid id && !context.AddressElements.Any(a => a.ObjectGuid == id))
        {
            return new ValidationResult("Address not found");
        }
        
        return ValidationResult.Success;
    }
}