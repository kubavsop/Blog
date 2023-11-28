using Blog.API.Common.Enums;

namespace Blog.API.Entities;

public class UserEdit
{
    public string FullName { get; set; }
    public DateTime? BirthDate { get; set; }
    public string Email { get; set; }
    public Gender Gender { get; set; }
    public string? PhoneNumber { get; set; }
}