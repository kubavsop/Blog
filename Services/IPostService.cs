using Blog.API.Entities;

namespace Blog.API.Services;

public interface IPostService
{
    Task<PostResponse> CreatePostAsync(CreatePost createPost);
    Task LikePostAsync(Guid id);
    Task UnlikePostAsync(Guid id);
}