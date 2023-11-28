using System.ComponentModel.DataAnnotations;
using Blog.API.Common.Enums;
using Blog.API.Controllers.Dto.CustomValidationAttributes;

namespace Blog.API.Controllers.Dto.Requests;

public class UserRegisterDto
{
    [Required]
    [MinLength(1)]
    public string FullName { get; set; }
    
    [Required]
    [MinLength(6)]
    [Password]
    public string Password { get; set; }
    
    [Date]
    public DateTime? BirthDate { get; set; }
    
    [Required]
    public Gender Gender { get; set; }
    
    [CustomPhone]
    public string? PhoneNumber { get; set; }
    
    [Required]
    [MinLength(1)]
    [EmailAddress]
    public string Email { get; set; }
}