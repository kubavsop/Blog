
using System.ComponentModel.DataAnnotations;

namespace Blog.API.Controllers.Dto.Responses;

public class TokenResponseDto
{
    [Required]
    [MinLength(1)]
    public string Token { get; set; }
}