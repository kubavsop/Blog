using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Blog.API.Controllers.Dto.CustomValidationAttributes;

namespace Blog.API.Controllers.Dto.Responses;

public class CommunityFullDto
{
    [Required]
    public Guid Id { get; set; }
    
    [Required]
    [Date]
    public DateTime CreateTime { get; set; }
    
    [Required]
    [MinLength(1)]
    public string Name { get; set; }
    
    public string? Description { get; set; }
    
    [Required]
    [DefaultValue(false)]
    public bool IsClosed { get; set; } 
    
    [Required]
    [DefaultValue(0)]
    public int SubscribersCount { get; set; }
    
    [Required]
    public IEnumerable<UserDto> Administrators { get; set; }
}