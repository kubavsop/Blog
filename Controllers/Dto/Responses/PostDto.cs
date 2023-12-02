using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Blog.API.Controllers.Dto.CustomValidationAttributes;

namespace Blog.API.Controllers.Dto.Responses;

public class PostDto
{
    [Required]
    public Guid Id { get; set; }
    
    [Required]
    [Date]
    public DateTime CreateTime { get; set; }
    
    [Required]
    [MinLength(1)]
    public string Title { get; set; }
    
    [Required]
    [MinLength(1)]
    public string Description { get; set; }
    
    [Required]
    public int ReadingTime { get; set; }
    
    public string? Image { get; set; }
    
    [Required]
    public Guid AuthorId { get; set; }
    
    [Required]
    [MinLength(1)]
    public string Author { get; set; }
    
    public Guid? CommunityId { get; set; }
    
    public string? CommunityName { get; set; }
    
    public Guid? AddressId { get; set; }
    
    [Required]
    [DefaultValue(0)]
    public int Likes { get; set; }
    
    [Required]
    [DefaultValue(false)]
    public bool HasLike { get; set; }
    
    [Required]
    [DefaultValue(0)]
    public int CommentsCount { get; set; }

    [Required]
    public IEnumerable<TagDto> Tags { get; set; }
}