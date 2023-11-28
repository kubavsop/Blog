using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Blog.API.Controllers.Dto.CustomValidationAttributes;

public class CustomPhoneAttribute: ValidationAttribute
{
    private const string Pattern = @"^((\+7)\s\(\d{3}\)\s\d{3}\-\d{2}\-\d{2})$";
    
    public override bool IsValid(object? value)
    {
        if (value is string phone && Regex.IsMatch(phone, Pattern))
        {
            return true;
        }

        ErrorMessage = "The PhoneNumber field is not a valid phone number. It should match the pattern: +7 (xxx) xxx-xx-xx.";
        return false;
    }
}