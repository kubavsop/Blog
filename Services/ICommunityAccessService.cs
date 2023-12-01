using Blog.API.Entities.Database;

namespace Blog.API.Services;

public interface ICommunityAccessService
{
    Task<List<Tag>> GetTags(IEnumerable<Guid> tagsId);
    Task CheckCommunityById(Guid? communityId);

    Task CheckCommunityByPost(Guid postId);

    Task<Community> GetCommunityAsync(Guid communityId);
}