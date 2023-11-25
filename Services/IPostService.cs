using Blog.API.Entities;

namespace Blog.API.Services;

public interface IPostService
{
    Task<PostResponse> CreatePostAsync(CreatePost createPost);
}