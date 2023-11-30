using Blog.API.Common.Enums;
using Blog.API.Common.Exceptions;
using Blog.API.Data;
using Blog.API.Entities;
using Blog.API.Entities.Database;
using Microsoft.EntityFrameworkCore;

namespace Blog.API.Services.Impl;

public class CommunityService : ICommunityService
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

    public async Task<RoleResponse> GetUserRoleAsync(Guid communityId)
    {
        await GetCommunityAsync(communityId);

        var userId = _tokenService.GetUserId();
        var communityUser = await _context.CommunityUser.FirstOrDefaultAsync(cu =>
            cu.CommunityId == communityId && cu.UserId == userId);

        return new RoleResponse
        {
            Role = communityUser?.Role
        };
    }

    public async Task SubscribeUserToCommunityAsync(Guid communityId)
    {
        var community = await GetCommunityAsync(communityId);
        var userId = _tokenService.GetUserId();
        var communityUser =
            await _context.CommunityUser.FirstOrDefaultAsync(cu =>
                cu.CommunityId == communityId && cu.UserId == userId);

        if (communityUser != null)
        {
            throw new UserRoleException(
                $"The user with id={userId} already has the {communityUser.Role} role in the community with id={communityId}");
        }

        community.SubscribersCount++;
        await _context.AddAsync(new CommunityUser
            {
                UserId = userId,
                CommunityId = communityId,
                Role = CommunityRole.Subscriber
            }
        );
        await _context.SaveChangesAsync();
    }

    public async Task UnsubscribeUserToCommunityAsync(Guid communityId)
    {
        var community = await GetCommunityAsync(communityId);
        var userId = _tokenService.GetUserId();
        var communityUser =
            await _context.CommunityUser.FirstOrDefaultAsync(cu =>
                cu.CommunityId == communityId && cu.UserId == userId);

        if (communityUser == null)
        {
            throw new UserRoleException(
                $"User with id={userId} not subscribed to the community with id={communityId}");
        }

        if (communityUser.Role == CommunityRole.Administrator)
        {
            throw new UserRoleException(
                $"The user with id={userId} is the group administrator"
            );
        }

        community.SubscribersCount--;
        _context.CommunityUser.Remove(communityUser);
        await _context.SaveChangesAsync();
    }

    public async Task<CommunityFull> GetInformationAboutCommunityAsync(Guid id)
    {
        var community = await GetCommunityAsync(id);
        var adminUsers = await GetAdminUsersAsync(id);

        return new CommunityFull
        {
            Id = community.Id,
            CreateTime = community.CreateTime,
            Name = community.Name,
            Description = community.Description,
            IsClosed = community.IsClosed,
            SubscribersCount = community.SubscribersCount,
            Administrators = adminUsers
        };
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

    private async Task<Community> GetCommunityAsync(Guid communityId)
    {
        var community = await _context.Communities.FirstOrDefaultAsync(c => c.Id == communityId);

        if (community == null)
        {
            throw new CommunityNotFoundException($"Community with id={communityId} not found in  database");
        }

        return community;
    }

    private async Task<IEnumerable<User>> GetAdminUsersAsync(Guid communityId)
    {
        var adminUsers = await _context.CommunityUser
            .Where(cu => cu.CommunityId == communityId && cu.Role == CommunityRole.Administrator)
            .Select(cu => cu.User)
            .ToListAsync();

        return adminUsers;
    }

    private async Task CheckNameExistenceAsync(string name)
    {
        if (await _context.Communities.AnyAsync(c => c.Name == name))
        {
            throw new CommunityAlreadyExistsException("Community with this name already exists");
        }
    }
}