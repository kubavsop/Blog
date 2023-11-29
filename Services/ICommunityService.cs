using Blog.API.Entities.Database;

namespace Blog.API.Services;

public interface ICommunityService
{
    Task<IEnumerable<Community>> GetCommunityListAsync();
}