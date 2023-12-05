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
    private readonly ISortingToolsService _sortingTools;
    private readonly ICommunityAccessService _communityAccess;

    public PostService(ITokenService tokenService, AppDbContext context, ICommunityAccessService communityAccess, ISortingToolsService sortingTools)
    {
        _tokenService = tokenService;
        _context = context;
        _communityAccess = communityAccess;
        _sortingTools = sortingTools;
    }

    public async Task<PostPagedList> GetPostsAsync(IEnumerable<Guid> tagsId, string? author, int? min,
        int? max, PostSorting sorting, bool onlyMyCommunities, int page, int size)
    {
        tagsId = tagsId.Distinct().ToList();

        await _sortingTools.GetTagsAsync(tagsId);

        var queryable = GetInitialPosts(onlyMyCommunities)
            .Where(p => !tagsId.Any() || p.Tags.Any(t => tagsId.Contains(t.Id)))
            .Where(p => author == null || p.Author.FullName.ToLower().Contains(author.ToLower()))
            .Where(p => (min == null || p.ReadingTime >= min) && (max == null || p.ReadingTime <= max));
        
        return await _sortingTools.GetPostPagedListAsync(queryable, sorting, page, size);
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
            Tags = await _sortingTools.GetTagsAsync(createPost.Tags.Distinct()),
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
            HasLike = await _sortingTools.HasUserLikedPostAsync(postId),
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
        var user = await _sortingTools.GetUserWithLikedPostsAsync();
        EnsureNoLikeExists(user, id);
        ChangeLikeCounter(true, post);
        user.LikedPosts.Add(post);
        await _context.SaveChangesAsync();
    }

    public async Task UnlikePostAsync(Guid id)
    {
        await _communityAccess.CheckCommunityByPost(id);
        var postToRemove = await GetPostByIdWithAuthorAsync(id);
        var user = await _sortingTools.GetUserWithLikedPostsAsync();
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