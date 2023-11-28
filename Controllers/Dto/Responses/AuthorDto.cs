using System.ComponentModel.DataAnnotations;
using Blog.API.Common.Enums;
using Blog.API.Controllers.Dto.CustomValidationAttributes;

namespace Blog.API.Controllers.Dto.Responses;

public class AuthorDto
{
    [Required]
    [MinLength(1)]
    public string FullName { get; set; }
    
    [Date]
    public DateTime? BirthDate { get; set; }
    
    [Required]
    public Gender Gender { get; set; }
    
    [Required]
    public int Posts { get; set; }
    
    [Required]
    public int Likes { get; set; }
    
    [Required]
    [Date]
    public DateTime Created { get; set; }
}