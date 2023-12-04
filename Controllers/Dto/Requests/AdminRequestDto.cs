using System.ComponentModel.DataAnnotations;

namespace Blog.API.Controllers.Dto.Requests;

public class AdminRequestDto
{
    [Required]
    public Guid UserId { get; set; }
}