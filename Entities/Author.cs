using Blog.API.Common.Enums;

namespace Blog.API.Entities;

public class Author
{
    public string FullName { get; set; }
    public DateTime? BirthDate { get; set; }
    public Gender Gender { get; set; }
    public int Posts { get; set; }
    public int Likes { get; set; }
    public DateTime Created { get; set; }
}