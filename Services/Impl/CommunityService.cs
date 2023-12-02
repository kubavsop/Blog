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
    private readonly ICommunityAccessService _communityAccess;

    public CommunityService(AppDbContext context, ITokenService tokenService, ICommunityAccessService communityAccess)
    {
        _context = context;
        _tokenService = tokenService;
        _communityAccess = communityAccess;
    }
    
    public async Task<PostPagedList> GetCommunitiesPosts(Guid id, IEnumerable<Guid> tags, PostSorting sorting, int page, int size)
    {
        await _communityAccess.CheckCommunityById(id);
        
        tags = tags.Distinct().ToList();
        
        await _communityAccess.GetTags(tags);

        var queryable = _context.Posts
            .Where(p => p.CommunityId == id)
            .Where(p => !tags.Any() || p.Tags.Any(t => tags.Contains(t.Id)));
        
        var sortedQueryable = sorting switch
        {
            PostSorting.CreateDesc => queryable.OrderByDescending(p => p.CreateTime),
            PostSorting.CreateAsc => queryable.OrderBy(p => p.CreateTime),
            PostSorting.LikeAsc => queryable.OrderBy(p => p.Likes),
            PostSorting.LikeDesc => queryable.OrderByDescending(p => p.Likes),
            _ => throw new ArgumentOutOfRangeException(nameof(sorting), sorting, null)
        };
        
        var result = await sortedQueryable
            .Skip((page - 1) * size)
            .Take(size)
            .Select(post => new PostInformation
            {
                Id = post.Id,
                CreateTime = post.CreateTime,
                Title = post.Title,
                Description = post.Description,
                ReadingTime = post.ReadingTime,
                Image = post.Image,
                AuthorId = post.AuthorId,
                Author = post.Author.FullName,
                CommunityId = post.CommunityId,
                CommunityName = post.Community != null ? post.Community.Name : null,
                AddressId = post.AddressId,
                Likes = post.Likes,
                CommentsCount = post.CommentsCount,
                Tags = post.Tags
            })
            .ToListAsync();

        if (result.Count == 0 && page != 1)
        {
            throw new InvalidPageException("Invalid value for attribute page");
        }

        foreach (var post in result)
        {
            post.HasLike = await HasUserLikedPost(post.Id);
        }
        
        var count = await queryable.CountAsync();

        return new PostPagedList
        {
            Posts = result,
            Pagination = new PageInfo
            {
                Count = (int)Math.Ceiling((double)count / size),
                Current = page,
                Size = size
            }
        };
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
        await _communityAccess.GetCommunityAsync(communityId);

        var userId = _tokenService.GetUserId();
        var communityUser = await _context.CommunityUser.FirstOrDefaultAsync(cu =>
            cu.CommunityId == communityId && cu.UserId == userId);

        return new RoleResponse
        {
            Role = communityUser?.Role
        };
    }

    public async Task<PostResponse> CreatePostAsync(CreatePost createPost, Guid communityId)
    {
        var tags = await _communityAccess.GetTags(createPost.Tags.Distinct());
        var community = await _communityAccess.GetCommunityAsync(communityId);
        var userId = _tokenService.GetUserId();

        if (!await _context.CommunityUser.AnyAsync(cu =>
                cu.CommunityId == communityId && cu.UserId == userId && cu.Role == CommunityRole.Administrator))
        {
            throw new CommunityAccessException(
                $"User Id={userId} is not able to post in community Id={communityId}");
        }

        var post = new Post
        {
            Title = createPost.Title,
            Description = createPost.Description,
            ReadingTime = createPost.ReadingTime,
            Image = createPost.Image,
            AddressId = createPost.AddressId,
            Community = community,
            Tags = tags,
            AuthorId = userId
        };

        await _context.Posts.AddAsync(post);
        await _context.SaveChangesAsync();
        return new PostResponse { PostId = post.Id };
    }

    public async Task SubscribeUserToCommunityAsync(Guid communityId)
    {
        var community = await _communityAccess.GetCommunityAsync(communityId);
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
        var community = await _communityAccess.GetCommunityAsync(communityId);
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
        var community = await _communityAccess.GetCommunityAsync(id);
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
    
    private async Task<IEnumerable<User>> GetAdminUsersAsync(Guid communityId)
    {
        var adminUsers = await _context.CommunityUser
            .Where(cu => cu.CommunityId == communityId && cu.Role == CommunityRole.Administrator)
            .Select(cu => cu.User)
            .ToListAsync();

        return adminUsers;
    }
    
    private async Task<bool> HasUserLikedPost(Guid postId)
    {
        if (!_tokenService.IsAuthenticated()) return false;
        var user = await GetUserWithLikedPostsAsync();
        return user.LikedPosts.Any(p => p.Id == postId);
    }
    
    private async Task<User> GetUserWithLikedPostsAsync()
    {
        var id = _tokenService.GetUserId();
        var user = await _context.Users
            .Include(user => user.LikedPosts)
            .FirstOrDefaultAsync(u => u.Id == id);

        if (user == null)
        {
            throw new UserNotFoundException("User not found");
        }

        return user;
    }

    private async Task CheckNameExistenceAsync(string name)
    {
        if (await _context.Communities.AnyAsync(c => c.Name == name))
        {
            throw new CommunityAlreadyExistsException("Community with this name already exists");
        }
    }
}