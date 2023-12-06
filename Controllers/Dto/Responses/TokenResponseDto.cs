
using System.ComponentModel.DataAnnotations;

namespace Blog.API.Controllers.Dto.Responses;

public class TokenResponseDto
{
    [Required]
    [MinLength(1)]
    public required string AccessToken { get; set; }
    
    [Required]
    [MinLength(1)]
    public required string RefreshToken { get; set; }
}