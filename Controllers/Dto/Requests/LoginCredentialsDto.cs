using System.ComponentModel.DataAnnotations;

namespace Blog.API.Controllers.Dto.Requests;

public class LoginCredentialsDto
{
    [Required]
    [MinLength(1)]
    [MaxLength(100)]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required]
    [MinLength(1)]
    [MaxLength(100)]
    public string Password { get; set; }
}