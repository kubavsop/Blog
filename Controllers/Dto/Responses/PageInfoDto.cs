using System.ComponentModel.DataAnnotations;

namespace Blog.API.Controllers.Dto.Responses;

public class PageInfoDto
{
    [Required]
    public int Size { get; set; }
    
    [Required]
    public int Count { get; set; }
    
    [Required]
    public int Current { get; set; }
}