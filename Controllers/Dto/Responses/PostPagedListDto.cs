using System.ComponentModel.DataAnnotations;

namespace Blog.API.Controllers.Dto.Responses;

public class PostPagedListDto
{
    [Required]
    public IEnumerable<PostDto> Posts { get; set; }
    
    [Required]
    public PageInfoDto Pagination { get; set; } 
}