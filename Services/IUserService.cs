using Blog.API.Controllers.Dto.Requests;
using Blog.API.Entities;

namespace Blog.API.Services;

public interface IUserService
{
    Task<TokenResponse> CreateUserAsync(User user);
    Task<TokenResponse> LoginUserAsync(LoginCredentials loginCredentials);

    Task LogoutUserAsync();
}