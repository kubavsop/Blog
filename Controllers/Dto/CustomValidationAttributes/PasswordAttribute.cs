using System.ComponentModel.DataAnnotations;

namespace Blog.API.Controllers.Dto.CustomValidationAttributes;

public class PasswordAttribute: ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value is string password && password.Any(char.IsDigit))
        {
            return true;
        }
        
        ErrorMessage = "Password requires at least one digit.";
        return false;
    }
}