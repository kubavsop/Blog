using Blog.API.Entities;

namespace Blog.API.Services;

public interface IJwtService
{
    TokenResponse CreateToken(string email);
}