namespace Blog.API.Entities;

public class PostPagedList
{
    public IEnumerable<PostInformation> Posts { get; set; }
    
    public PageInfo Pagination { get; set; } 
}