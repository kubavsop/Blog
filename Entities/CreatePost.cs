namespace Blog.API.Entities;

public class CreatePost
{
    public string Title { get; set; }
    public string Description { get; set; }
    public int ReadingTime { get; set; }
    public string? Image { get; set; }
    
    public Guid? AddressId { get; set; }
    public IEnumerable<Guid> Tags { get; set; }
}