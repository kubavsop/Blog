
namespace Blog.API.Entities.Database;

public class Author
{
    
    public Guid UserId { get; set; }
    public int Likes { get; set; }
    public DateTime Created { get; set; } = DateTime.UtcNow;
    public User User { get; set; }
    public ICollection<Post> Posts { get; set; }
}