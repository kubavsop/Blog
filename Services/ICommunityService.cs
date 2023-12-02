using Blog.API.Common.Enums;
using Blog.API.Entities;
using Blog.API.Entities.Database;

namespace Blog.API.Services;

public interface ICommunityService
{
    Task<IEnumerable<Community>> GetCommunityListAsync();

    Task<IEnumerable<CommunityUser>> GetUserCommunitiesAsync();
    Task<RoleResponse> GetUserRoleAsync(Guid communityId);
    Task<PostResponse> CreatePostAsync(CreatePost createPost, Guid communityId);
    Task SubscribeUserToCommunityAsync(Guid communityId);
    Task UnsubscribeUserToCommunityAsync(Guid communityId);
    Task<CommunityFull> GetInformationAboutCommunityAsync(Guid id);
    Task<PostPagedList> GetCommunitiesPosts(Guid id, IEnumerable<Guid> tags, PostSorting sorting, int page, int size);
    Task CreateCommunityAsync(Community community);
}