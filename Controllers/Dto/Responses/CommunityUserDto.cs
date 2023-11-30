using System.ComponentModel.DataAnnotations;
using Blog.API.Common.Enums;

namespace Blog.API.Controllers.Dto.Responses;

public class CommunityUserDto
{
    [Required]
    public Guid UserId { get; set; }
    
    [Required]
    public Guid CommunityId { get; set; }
    
    [Required]
    public CommunityRole Role { get; set; }
}