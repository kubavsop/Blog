using System.Security.Claims;
using Blog.API.Data;
using Blog.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Blog.API.Services.Impl;

public class TokenService: ITokenService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly AppDbContext _context;

    public TokenService(IHttpContextAccessor accessor, AppDbContext context)
    {
        _httpContextAccessor = accessor;
        _context = context;
    }

    public async Task InvalidateTokenAsync()
    {
        var tokenId = GetTokenId();
        await _context.Tokens.AddAsync(new InvalidTokens { Id = tokenId });
        await _context.SaveChangesAsync();
    }

    public async Task<bool> CheckTokenAsync()
    {
        var tokenId = GetTokenId();
        return await _context.Tokens.AnyAsync(t => t.Id == tokenId);
    }
    public Guid GetUserId()
    {
        return Guid.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.Name)!);
    }
    
    private Guid GetTokenId()
    {
        return Guid.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }
}