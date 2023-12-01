using Blog.API.Controllers.Dto.Responses;
using Blog.API.Entities.Database;

namespace Blog.API.Entities;

public class PostFull
{
    public Guid Id { get; set; }
    
    public DateTime CreateTime { get; set; }
    
    public string Title { get; set; }
    
    public string Description { get; set; }
    
    public int ReadingTime { get; set; }
    
    public string? Image { get; set; }
    
    public Guid AuthorId { get; set; }
    
    public string Author { get; set; }
    
    public Guid? CommunityId { get; set; }
    
    public string? CommunityName { get; set; }
    
    public Guid? AddressId { get; set; }
    
    public int Likes { get; set; }
    
    public bool HasLike { get; set; }
    
    public int CommentsCount { get; set; }
    
    public IEnumerable<Tag> Tags { get; set; }
    
    public IEnumerable<Comment> Comments { get; set; }
}