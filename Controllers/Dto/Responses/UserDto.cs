using Blog.API.Enums;

namespace Blog.API.Controllers.Dto.Responses;

public class UserDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; }
    public DateTime? BirthDate { get; set; }

    public DateTime CreateTime { get; set; } = DateTime.UtcNow;
    public Gender Gender { get; set; }
    public string? PhoneNumber { get; set; }
    public string Email { get; set; }
}