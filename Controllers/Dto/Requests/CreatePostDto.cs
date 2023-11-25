namespace Blog.API.Controllers.Dto.Requests;

public class CreatePostDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public int ReadingTime { get; set; }
    public string Image { get; set; }
    public string AddresId { get; set; }
    public IEnumerable<Guid> Tags { get; set; }
}