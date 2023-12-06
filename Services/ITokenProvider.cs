using Blog.API.Entities;

namespace Blog.API.Services;

public interface ITokenProvider
{
    TokenResponse CreateTokenResponse(Guid id);
}