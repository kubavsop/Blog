using Blog.API.Entities.Database;

namespace Blog.API.Entities;

public class CommunityFull
{
    public Guid Id { get; set; }
    
    public DateTime CreateTime { get; set; }
    
    public string Name { get; set; }
    
    public string? Description { get; set; }

    public bool IsClosed { get; set; } 
    
    public int SubscribersCount { get; set; }
    
    public IEnumerable<User> Administrators { get; set; }
}