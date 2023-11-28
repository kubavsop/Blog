using System.ComponentModel.DataAnnotations;

namespace Blog.API.Controllers.Dto.CustomValidationAttributes;

public class DateAttribute: ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value is DateTime date && date < DateTime.Now)
        {
            return true;
        }
        
        ErrorMessage = "Date can't be later than today";
        return false;
    }
}