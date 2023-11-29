using System.ComponentModel.DataAnnotations;
using Blog.API.Common.Enums;

namespace Blog.API.Controllers.Dto.Responses;

public class SearchAddressDto
{
    [Required]
    public long ObjectId { get; set; }
    
    [Required]
    public Guid ObjectGuid { get; set; }
    
    [Required]
    public string Text { get; set; }
    
    [Required]
    public GarAddressLevel ObjectLevel { get; set; }
    
    [Required]
    public string ObjectLevelText { get; set; }
}