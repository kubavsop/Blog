using Blog.API.Entities.Database;

namespace Blog.API.Services;

public interface ITagService
{
    Task<IEnumerable<Tag>> GetTagsAsync();
    Task CreateTagAsync(Tag tag);
}