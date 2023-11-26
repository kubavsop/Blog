namespace Blog.API.Controllers.Dto.Requests;

public class CreateCommentDto
{
    public string Content { get; set; }
    public Guid? ParentId { get; set; }
}