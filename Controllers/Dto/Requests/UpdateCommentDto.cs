using System.ComponentModel.DataAnnotations;

namespace Blog.API.Controllers.Dto.Requests;

public class UpdateCommentDto
{
    [Required]
    [MinLength(1)]
    public string Content { get; set; }
}