using System.ComponentModel.DataAnnotations;

namespace Blog.API.Controllers.Dto.Requests;

public class CreateCommunityDto
{
    [Required]
    [MinLength(1)]
    public string Name { get; set; }
    
    public string Description { get; set; }
    
    [Required]
    public Boolean IsClosed { get; set; }
}