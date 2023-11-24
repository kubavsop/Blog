using Blog.API.Enums;

namespace Blog.API.Controllers.Dto.Requests;

public class UserEditDto
{
    public string FullName { get; set; }
    public DateTime? BirthDate { get; set; }
    public string Email { get; set; }
    public Gender Gender { get; set; }
    public string? PhoneNumber { get; set; }
}   