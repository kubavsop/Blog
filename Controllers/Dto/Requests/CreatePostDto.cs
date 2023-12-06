using System.ComponentModel.DataAnnotations;
using Blog.API.Controllers.Dto.CustomValidationAttributes;
using Blog.API.Data;

namespace Blog.API.Controllers.Dto.Requests;

public class CreatePostDto
{
    [Required]
    [MinLength(5)]
    [MaxLength(100)]
    public string Title { get; set; }
    
    [Required]
    [MinLength(5)]
    [MaxLength(1000)]
    public string Description { get; set; }
    
    [Required]
    [Range(0, int.MaxValue)]
    public int ReadingTime { get; set; }
    
    [Url]
    [MaxLength(1000)]
    public string? Image { get; set; }
    
    [Address]
    public Guid? AddressId { get; set; }
    
    [Required]
    [MinLength(1)]
    [MaxLength(100)]
    public IEnumerable<Guid> Tags { get; set; }
}