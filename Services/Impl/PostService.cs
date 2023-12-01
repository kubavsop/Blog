using Blog.API.Common.Exceptions;
using Blog.API.Data;
using Blog.API.Entities;
using Blog.API.Entities.Database;
using Microsoft.EntityFrameworkCore;

namespace Blog.API.Services.Impl;

public class PostService: IPostService
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

    public async Task LikePostAsync(Guid id)
    {
        await _communityAccess.CheckCommunityByPost(id);
        var post = await GetPostByIdAsync(id);
        var user = await _tokenService.GetUserWithLikedPostsAsync();
        EnsureNoLikeExists(user, id);
        ChangeLikeCounter(true, post);
        user.LikedPosts.Add(post);
        await _context.SaveChangesAsync();
    }

    public async Task UnlikePostAsync(Guid id)
    {
        await _communityAccess.CheckCommunityByPost(id);
        var postToRemove = await GetPostByIdAsync(id);
        var user = await _tokenService.GetUserWithLikedPostsAsync();
        EnsureLikeExists(user, id);
        ChangeLikeCounter(false, postToRemove);
        user.LikedPosts.Remove(postToRemove);
        await _context.SaveChangesAsync();
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

    private async Task<Post> GetPostByIdAsync(Guid id)
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