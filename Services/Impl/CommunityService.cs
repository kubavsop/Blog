using Blog.API.Data;
using Blog.API.Entities.Database;
using Microsoft.EntityFrameworkCore;

namespace Blog.API.Services.Impl;

public class CommunityService: ICommunityService
{
    private readonly ITokenService _tokenService;
    private readonly AppDbContext _context;

    public CommunityService(AppDbContext context, ITokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    public async Task<IEnumerable<Community>> GetCommunityListAsync()
    {
        return await _context.Communities.ToListAsync();
    }

    public async Task<IEnumerable<CommunityUser>> GetUserCommunities()
    {
        var userId = _tokenService.GetUserId();
        return await _context.CommunityUser.Where(cu => cu.UserId == userId).ToListAsync();
    }
}