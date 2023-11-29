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

    public int CommentsCount { get; set; } 
    
    public Community? Community { get; set; }
    public User Author { get; set; }
    
    public ICollection<User> LikedUsers { get; set; } = new List<User>();
    public ICollection<Tag> Tags { get; set; } = new List<Tag>();
    
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
}