using Blog.API.Common.Enums;
using Blog.API.Common.Exceptions;
using Blog.API.Data;
using Blog.API.Entities;
using Blog.API.Entities.Database;
using Microsoft.EntityFrameworkCore;

namespace Blog.API.Services.Impl;

public class SortingToolsService: ISortingToolsService
{
    private readonly AppDbContext _context;
    private readonly ITokenService _tokenService;

    public SortingToolsService(AppDbContext context, ITokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    public async Task<PostPagedList> GetPostPagedListAsync(IQueryable<Post> queryable, PostSorting sorting, int page, int size)
    {
        var sortedQueryable = SortPosts(queryable, sorting);

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
            post.HasLike = await HasUserLikedPostAsync(post.Id);
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
    
    public IQueryable<Post> SortPosts(IQueryable<Post> queryable, PostSorting sorting)
    {
        var sortedQueryable = sorting switch
        {
            PostSorting.CreateDesc => queryable.OrderByDescending(p => p.CreateTime),
            PostSorting.CreateAsc => queryable.OrderBy(p => p.CreateTime),
            PostSorting.LikeAsc => queryable.OrderBy(p => p.Likes),
            PostSorting.LikeDesc => queryable.OrderByDescending(p => p.Likes),
            _ => throw new ArgumentOutOfRangeException(nameof(sorting), sorting, null)
        };

        return sortedQueryable;
    }

    public async Task<List<Tag>> GetTagsAsync(IEnumerable<Guid> tagsId)
    {

        var tags = await _context.Tags
            .Where(t => tagsId.Contains(t.Id))
            .ToListAsync();

        if (tags.Count != tagsId.Count())
        {
            throw new TagNotFoundException("Tag not found");
        }

        return tags;
    }
    
    public async Task<bool> HasUserLikedPostAsync(Guid postId)
    {
        if (!_tokenService.IsAuthenticated()) return false;
        var user = await GetUserWithLikedPostsAsync();
        return user.LikedPosts.Any(p => p.Id == postId);
    }
    
    public async Task<User> GetUserWithLikedPostsAsync()
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
}