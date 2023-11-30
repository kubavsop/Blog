using Blog.API.Common.Enums;
using Blog.API.Common.Exceptions;
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

    public async Task<IEnumerable<CommunityUser>> GetUserCommunitiesAsync()
    {
        var userId = _tokenService.GetUserId();
        return await _context.CommunityUser.Where(cu => cu.UserId == userId).ToListAsync();
    }

    public async Task CreateCommunityAsync(Community community)
    {
        await CheckNameExistenceAsync(community.Name);
        await _context.Communities.AddAsync(community);
        
        var communityUser = new CommunityUser
        {
            UserId = _tokenService.GetUserId(),
            Community = community,
            Role = CommunityRole.Administrator
        };
        
        await _context.CommunityUser.AddAsync(communityUser);
        
        await _context.SaveChangesAsync();
    }

    private async Task CheckNameExistenceAsync(string name)
    {
        if (await _context.Communities.AnyAsync(c => c.Name == name))
        {
            throw new CommunityAlreadyExistsException("Community with this name already exists");
        }
    }
}