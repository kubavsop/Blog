using System.ComponentModel.DataAnnotations;

namespace Blog.API.Controllers.Dto.Requests;

public class RefreshRequestDto
{
    [Required]
    [MinLength(1)]
    [MaxLength(1000)]
    public required string Token { get; set; }
}