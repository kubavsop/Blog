using Blog.API.Enums;

namespace Blog.API.Controllers.Dto.Responses;

public class AuthorDto
{
    public string FullName { get; set; }
    public DateTime? BirthDate { get; set; }
    public Gender Gender { get; set; }
    public int Posts { get; set; }
    public int Likes { get; set; }
    public DateTime Created { get; set; }
}