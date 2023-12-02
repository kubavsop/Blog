using Blog.API.Common.Enums;
using Blog.API.Entities;
using Blog.API.Entities.Database;

namespace Blog.API.Services;

public interface IPostService
{
    Task<PostResponse> CreatePostAsync(CreatePost createPost);

    Task<PostInformation> GetInformationAboutPost(Guid postId);
    Task LikePostAsync(Guid id);
    Task UnlikePostAsync(Guid id);
    Task<PostPagedList> GetPostsAsync(IEnumerable<Guid> tagsId, string? author, int? min, int? max,
        PostSorting sorting, bool onlyMyCommunities, int page, int size);
}