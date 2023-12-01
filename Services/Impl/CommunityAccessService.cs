using Blog.API.Common.Exceptions;
using Blog.API.Data;
using Blog.API.Entities.Database;
using Microsoft.EntityFrameworkCore;

namespace Blog.API.Services.Impl;

public class CommunityAccessService : ICommunityAccessService
{
    private readonly AppDbContext _context;
    private readonly ITokenService _tokenService;

    public CommunityAccessService(AppDbContext context, ITokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    public async Task CheckCommunityById(Guid? communityId)
    {
        if (communityId == null) return;

        var userId = _tokenService.GetUserId();
        var community = await GetCommunityAsync(communityId.GetValueOrDefault());

        if (community.IsClosed &&
            !await _context.CommunityUser.AnyAsync(cu => cu.CommunityId == communityId && cu.UserId == userId))
        {
            throw new CommunityAccessException(
                $"Access to closed community comment with id={communityId} is forbidden for user Id={userId}");
        }
    }

    public async Task CheckCommunityByPost(Guid postId)
    {
        var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == postId);

        if (post == null)
        {
            throw new PostNotFoundException($"Post with id={postId} not found in database");
        }

        await CheckCommunityById(post.CommunityId);
    }
    
    

    public async Task<List<Tag>> GetTags(IEnumerable<Guid> tagsId)
    {
        var tagsIdList = tagsId.ToList();

        var tags = await _context.Tags
            .Where(t => tagsIdList.Contains(t.Id))
            .ToListAsync();

        if (tags.Count != tagsIdList.Count)
        {
            throw new TagNotFoundException("Tag not found");
        }

        return tags;
    }

    public async Task<Community> GetCommunityAsync(Guid communityId)
    {
        var community = await _context.Communities.FirstOrDefaultAsync(c => c.Id == communityId);

        if (community == null)
        {
            throw new CommunityNotFoundException($"Community with id={communityId} not found in  database");
        }

        return community;
    }
}