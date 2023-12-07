using System.Runtime.InteropServices.JavaScript;
using System.Security.Claims;
using Blog.API.Common.Exceptions;
using Blog.API.Data;
using Blog.API.Entities;
using Blog.API.Entities.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace Blog.API.Services.Impl;

public class TokenService: ITokenService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly IDistributedCache _distributedCache;

    public TokenService(IHttpContextAccessor accessor, AppDbContext context, IDistributedCache distributedCache, IConfiguration configuration)
    {
        _httpContextAccessor = accessor;
        _context = context;
        _distributedCache = distributedCache;
        _configuration = configuration;
    }

    public async Task InvalidateTokenAsync()
    {
        var expireMinutes = _configuration.GetValue<double>("AppSettings:AccessTokenExpireMinutes");
        var options = new DistributedCacheEntryOptions()
            .SetAbsoluteExpiration(DateTimeOffset.Now.AddMinutes(expireMinutes));
        
        var tokenId = GetTokenId().ToString();
        await _distributedCache.SetStringAsync(tokenId, "1", options);
    }

    public async Task<User> GetUserAsync()
    {
        var id = GetUserId();
        return await GetUserById(id);
    }

    public bool CheckToken()
    {
        var tokenId = GetTokenId().ToString();
        return _distributedCache.Get(tokenId) != null;
    }
    
    private async Task<User> GetUserById(Guid id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (user == null)
        {
            throw new UserNotFoundException("User not found");
        }
        return user;
    }
    
    public Guid GetUserId()
    {
        if (_httpContextAccessor.HttpContext?.User.Identity is { IsAuthenticated: true } && !CheckToken())
        {
            return Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name)!);
        }
        throw new UserNotAuthorizedException();
    }

    public bool IsAuthenticated()
    {
        return _httpContextAccessor.HttpContext?.User.Identity is { IsAuthenticated: true } && !CheckToken();
    }
    
    private Guid GetTokenId()
    {
        if (_httpContextAccessor.HttpContext?.User.Identity is { IsAuthenticated: true })
        {
            return Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        }
        throw new UserNotAuthorizedException();
    }
}