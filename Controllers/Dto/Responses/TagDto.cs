using System.ComponentModel.DataAnnotations;
using Blog.API.Controllers.Dto.CustomValidationAttributes;

namespace Blog.API.Controllers.Dto.Responses;

public class TagDto
{
    [Required]
    public Guid Id { get; set; }
    
    [Required]
    [Date]
    public DateTime CreateTime { get; set; }
    
    [Required]
    [MinLength(1)]
    public string Name { get; set; }
}