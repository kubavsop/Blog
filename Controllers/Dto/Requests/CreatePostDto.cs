using System.ComponentModel.DataAnnotations;

namespace Blog.API.Controllers.Dto.Requests;

public class CreatePostDto
{
    [Required]
    [MinLength(5)]
    public string Title { get; set; }
    
    [Required]
    [MinLength(5)]
    public string Description { get; set; }
    
    [Required]
    public int ReadingTime { get; set; }
    
    [Url]
    public string? Image { get; set; }
    
    public Guid? AddressId { get; set; }
    
    [Required]
    [MinLength(1)]
    public IEnumerable<Guid> Tags { get; set; }
}