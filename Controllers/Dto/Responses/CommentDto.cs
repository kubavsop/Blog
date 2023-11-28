using System.ComponentModel.DataAnnotations;
using Blog.API.Controllers.Dto.CustomValidationAttributes;

namespace Blog.API.Controllers.Dto.Responses;

public class CommentDto
{
    [Required]
    public Guid Id { get; set; }
    
    [Required]
    [Date]
    public DateTime CreateTime { get; set; }
    
    [Required]
    [MinLength(1)]
    public string Content { get; set; }
    
    [Date]
    public DateTime? ModifiedDate { get; set; }
    
    [Date]
    public DateTime? DeleteDate { get; set; }
    
    [Required]
    public Guid AuthorId { get; set; }
    
    [Required]
    [MinLength(1)]
    public string Author { get; set; }
    
    [Required]
    public int SubComments { get; set; }
}