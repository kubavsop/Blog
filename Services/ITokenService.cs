namespace Blog.API.Services;

public interface ITokenService
{
    Task InvalidateTokenAsync();

    Guid GetUserId();
    Task<bool> CheckTokenAsync();
}