using System.ComponentModel.DataAnnotations;

namespace Blog.API.Controllers.Dto.Responses;

public class PostResponseDto
{
    [Required]
    public Guid PostId { get; set; }
}