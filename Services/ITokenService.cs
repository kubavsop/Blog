using Blog.API.Entities.Database;

namespace Blog.API.Services;

public interface ITokenService
{
    Task InvalidateTokenAsync();

    Task<User> GetUserAsync();

    Task<User> GetUserWithLikedPostsAsync();
    Task<bool> CheckTokenAsync();
}