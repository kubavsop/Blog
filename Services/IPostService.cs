using Blog.API.Entities;
using Blog.API.Entities.Database;

namespace Blog.API.Services;

public interface IPostService
{
    Task<PostResponse> CreatePostAsync(CreatePost createPost);

    Task<PostFull> GetInformationAboutPost(Guid postId);
    Task LikePostAsync(Guid id);
    Task UnlikePostAsync(Guid id);
}