namespace Blog.API.Services;

public interface ITokenService
{
    Task InvalidateTokenAsync();

    string GetUserEmail();
    Task<bool> CheckTokenAsync();
}