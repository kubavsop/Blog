namespace Blog.API.Entities.Database;

public class Tag
{
    public Guid Id { get; set; }
    public DateTime CreateTime { get; set; } = DateTime.UtcNow;
    public String Name { get; set; }
    public ICollection<Post>? Posts { get; set; }
}