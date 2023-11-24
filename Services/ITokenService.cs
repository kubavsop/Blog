namespace Blog.API.Services;

public interface ITokenService
{
    Task InvalidateTokenAsync();
    Task<bool> CheckTokenAsync();
}