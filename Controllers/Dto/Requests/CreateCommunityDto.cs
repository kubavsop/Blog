using System.ComponentModel.DataAnnotations;

namespace Blog.API.Controllers.Dto.Requests;

public class CreateCommunityDto
{
    [Required]
    [MinLength(1)]
    [MaxLength(100)]
    public string Name { get; set; }
    
    [MinLength(1)]
    [MaxLength(1000)]
    public string Description { get; set; }
    
    [Required]
    public Boolean IsClosed { get; set; }
}