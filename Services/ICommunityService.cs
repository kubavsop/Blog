using Blog.API.Entities;
using Blog.API.Entities.Database;

namespace Blog.API.Services;

public interface ICommunityService
{
    Task<IEnumerable<Community>> GetCommunityListAsync();

    Task<IEnumerable<CommunityUser>> GetUserCommunitiesAsync();
    Task<RoleResponse> GetUserRoleAsync(Guid communityId);
    Task SubscribeUserToCommunityAsync(Guid communityId);
    Task UnsubscribeUserToCommunityAsync(Guid communityId);
    Task<CommunityFull> GetInformationAboutCommunityAsync(Guid id);

    Task CreateCommunityAsync(Community community);
}