using Blog.API.Entities;
using Blog.API.Entities.Database;

namespace Blog.API.Services;

public interface ICommunityService
{
    Task<IEnumerable<Community>> GetCommunityListAsync();

    Task<IEnumerable<CommunityUser>> GetUserCommunitiesAsync();

    Task CreateCommunityAsync(Community community);
}