using Blog.API.Enums;

namespace Blog.API.Controllers.Dto.Requests;

public class UserRegisterDto
{
    public string FullName { get; set; }
    public string Password { get; set; }
    public DateTime? BirthDate { get; set; }
    public Gender Gender { get; set; }
    public string? PhoneNumber { get; set; }
    public string Email { get; set; }
}