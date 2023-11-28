using System.ComponentModel.DataAnnotations;
using Blog.API.Common.Enums;
using Blog.API.Controllers.Dto.CustomValidationAttributes;

namespace Blog.API.Controllers.Dto.Responses;

public class UserDto
{
    [Required]
    public Guid Id { get; set; }
    
    [Required]
    [Date]
    public DateTime CreateTime { get; set; }
    
    [Required]
    [MinLength(1)]
    public string FullName { get; set; }
    
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