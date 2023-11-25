using Blog.API.Enums;

namespace Blog.API.Entities.Database;

public class User
{
    public Guid Id { get; set; }
    public string FullName { get; set; }
    public string Password { get; set; }
    public DateTime? BirthDate { get; set; }

    public DateTime CreateTime { get; set; } = DateTime.UtcNow;
    public Gender Gender { get; set; }
    public string? PhoneNumber { get; set; }
    public string Email { get; set; }
    
    public ICollection<Post>? LikedPosts { get; set; }
    public Author? Author { get; set; }
}   