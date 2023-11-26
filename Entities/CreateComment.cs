namespace Blog.API.Entities;

public class CreateComment
{
    public string Content { get; set; }
    public Guid? ParentId { get; set; }
}