namespace Blog.API.Entities.Database;

public class Post
{
    public Guid Id { get; set; }
    public DateTime CreateTime { get; set; } = DateTime.UtcNow;
    public string Title { get; set; }
    public string Description { get; set; }
    public int ReadingTime { get; set; }
    public string? Image { get; set; }
    public int Likes { get; set; }
    public Author Author { get; set; }
    public ICollection<User>? LikedUsers { get; set; }
    public ICollection<Tag> Tags { get; set; }
}