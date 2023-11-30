using System.Security.Claims;
using Blog.API.Common.Exceptions;
using Blog.API.Data;
using Blog.API.Entities;
using Blog.API.Entities.Database;
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

    public async Task<User> GetUserAsync()
    {
        var id = GetUserId();
        return await GetUserById(id);
    }

    public async Task<bool> CheckTokenAsync()
    {
        var tokenId = GetTokenId();
        return await _context.Tokens.AnyAsync(t => t.Id == tokenId);
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

    public async Task<User> GetUserWithLikedPostsAsync()
    {
        var id = GetUserId();
        var user = await _context.Users
            .Include(user => user.LikedPosts)
            .FirstOrDefaultAsync(u => u.Id == id);
        
        if (user == null)
        {
            throw new UserNotFoundException("User not found");
        }
        
        return user;
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