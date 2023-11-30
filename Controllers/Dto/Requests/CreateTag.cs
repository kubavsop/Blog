using System.ComponentModel.DataAnnotations;

namespace Blog.API.Controllers.Dto.Requests;

public class CreateTag
{
    [Required]
    [MinLength(1)]
    public string Name { get; set; }
}