using Blog.API.Entities;
using Blog.API.Entities.Database;

namespace Blog.API.Services;

public interface IUserService
{
    Task<TokenResponse> CreateUserAsync(User user);
    Task<TokenResponse> LoginUserAsync(LoginCredentials loginCredentials);
    Task<TokenResponse> RefreshAsync(string token);
    Task<User> GetUserAsync();

    Task EditUserAsync(UserEdit userEdit);

    Task LogoutUserAsync();
}