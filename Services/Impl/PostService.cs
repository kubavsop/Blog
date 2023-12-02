using Blog.API.Common.Enums;
using Blog.API.Common.Exceptions;
using Blog.API.Data;
using Blog.API.Entities;
using Blog.API.Entities.Database;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;

namespace Blog.API.Services.Impl;

public class PostService : IPostService
{
    private readonly ITokenService _tokenService;
    private readonly AppDbContext _context;
    private readonly ICommunityAccessService _communityAccess;

    public PostService(ITokenService tokenService, AppDbContext context, ICommunityAccessService communityAccess)
    {
        _tokenService = tokenService;
        _context = context;
        _communityAccess = communityAccess;
    }

    public async Task<PostPagedList> GetPostsAsync(IEnumerable<Guid> tagsId, string? author, int? min,
        int? max, PostSorting sorting, bool onlyMyCommunities, int page, int size)
    {
        tagsId = tagsId.Distinct().ToList();

        await _communityAccess.GetTags(tagsId);

        var queryable = GetInitialPosts(onlyMyCommunities)
            .Where(p => !tagsId.Any() || p.Tags.Any(t => tagsId.Contains(t.Id)))
            .Where(p => author == null || p.Author.FullName.ToLower().Contains(author.ToLower()))
            .Where(p => (min == null || p.ReadingTime >= min) && (max == null || p.ReadingTime <= max));

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

    public async Task<PostResponse> CreatePostAsync(CreatePost createPost)
    {
        var post = new Post
        {
            Title = createPost.Title,
            Description = createPost.Description,
            ReadingTime = createPost.ReadingTime,
            Image = createPost.Image,
            AddressId = createPost.AddressId,
            Tags = await _communityAccess.GetTags(createPost.Tags.Distinct()),
            AuthorId = _tokenService.GetUserId()
        };

        await _context.Posts.AddAsync(post);
        await _context.SaveChangesAsync();
        return new PostResponse { PostId = post.Id };
    }

    public async Task<PostInformation> GetInformationAboutPost(Guid postId)
    {
        var post = await GetFullPostById(postId);

        await _communityAccess.CheckCommunityById(post.CommunityId);

        return new PostInformation
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
            CommunityName = post.Community?.Name,
            AddressId = post.AddressId,
            Likes = post.Likes,
            HasLike = await HasUserLikedPost(postId),
            CommentsCount = post.CommentsCount,
            Tags = post.Tags,
            Comments = await _context.Comments
                .Include(c => c.Author)
                .Where(c => c.PostId == postId && c.ParentId == null)
                .OrderByDescending(c => c.CreateTime)
                .ToListAsync()
        };
    }

    public async Task LikePostAsync(Guid id)
    {
        await _communityAccess.CheckCommunityByPost(id);
        var post = await GetPostByIdWithAuthorAsync(id);
        var user = await GetUserWithLikedPostsAsync();
        EnsureNoLikeExists(user, id);
        ChangeLikeCounter(true, post);
        user.LikedPosts.Add(post);
        await _context.SaveChangesAsync();
    }

    public async Task UnlikePostAsync(Guid id)
    {
        await _communityAccess.CheckCommunityByPost(id);
        var postToRemove = await GetPostByIdWithAuthorAsync(id);
        var user = await GetUserWithLikedPostsAsync();
        EnsureLikeExists(user, id);
        ChangeLikeCounter(false, postToRemove);
        user.LikedPosts.Remove(postToRemove);
        await _context.SaveChangesAsync();
    }

    private IQueryable<Post> GetInitialPosts(bool onlyMyCommunities)
    {
        if (!_tokenService.IsAuthenticated())
        {
            var publicCommunities = _context.Communities
                .Where(c => c.IsClosed == false)
                .Select(c => c.Id);

            return _context.Posts
                .Where(p => p.CommunityId == null || publicCommunities.Contains(p.CommunityId ?? Guid.Empty));
        }

        var userId = _tokenService.GetUserId();

        var myCommunities = _context.CommunityUser
            .Where(cu => cu.UserId == userId)
            .Select(cu => cu.CommunityId);

        if (onlyMyCommunities)
        {
            return _context.Posts.Where(p =>
                p.CommunityId != null && myCommunities.Contains(p.CommunityId ?? Guid.Empty));
        }

        var communities = _context.Communities
            .Where(c => c.IsClosed == false || myCommunities.Contains(c.Id))
            .Select(c => c.Id);

        return _context.Posts
            .Where(p => p.CommunityId == null || communities.Contains(p.CommunityId ?? Guid.Empty));
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

    private void ChangeLikeCounter(bool isLike, Post post)
    {
        var increment = isLike ? 1 : -1;
        post.Author.Likes += increment;
        post.Likes += increment;
    }

    private void EnsureNoLikeExists(User user, Guid postId)
    {
        if (user.LikedPosts.Any(p => p.Id == postId))
        {
            throw new LikeExistsException("Like on this post already set by user");
        }
    }

    private void EnsureLikeExists(User user, Guid postId)
    {
        if (user.LikedPosts.All(p => p.Id != postId))
        {
            throw new LikeExistsException("There are no like from user by this post");
        }
    }

    private async Task<Post> GetFullPostById(Guid id)
    {
        var post = await _context.Posts
            .Include(post => post.Author)
            .Include(post => post.Community)
            .Include(post => post.Tags)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (post == null)
        {
            throw new PostNotFoundException($"Post with id={id} not found in database");
        }

        return post;
    }

    private async Task<Post> GetPostByIdWithAuthorAsync(Guid id)
    {
        var post = await _context.Posts
            .Include(post => post.Author)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (post == null)
        {
            throw new PostNotFoundException($"Post with id={id} not found in database");
        }

        return post;
    }
}