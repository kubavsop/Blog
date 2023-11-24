using Blog.API.Entities;

namespace Blog.API.Services;

public interface ITokenProvider
{
    TokenResponse CreateToken(Guid id);
}