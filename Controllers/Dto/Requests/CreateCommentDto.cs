using System.ComponentModel.DataAnnotations;

namespace Blog.API.Controllers.Dto.Requests;

public class CreateCommentDto
{
    [Required]
    [MinLength(1)]
    [MaxLength(1000)]
    public string Content { get; set; }
    public Guid? ParentId { get; set; }
}