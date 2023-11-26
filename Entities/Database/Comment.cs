using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Blog.API.Entities.Database;

public class Comment
{
    public Guid Id { get; set; }
    public Guid? ParentId { get; set; }
    public DateTime CreateTime { get; set; } = DateTime.UtcNow;
    public string Content { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public DateTime? DeleteDate { get; set; }
    public int SubComments { get; set; }
    
    public Guid AuthorId { get; set; }
    public Guid PostId { get; set; }
    
    public User Author { get; set; }
    public Post Post { get; set; }
}