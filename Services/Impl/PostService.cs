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

    public PostService(ITokenService tokenService, AppDbContext context)
    {
        _tokenService = tokenService;
        _context = context;
    }

    public async Task<PostResponse> CreatePostAsync(CreatePost createPost)
    {
        
        if (createPost.AddressId != null)
        {
            await CheckAddressExistence(createPost.AddressId.GetValueOrDefault());
        }
        
        var post = new Post
        {
            Title = createPost.Title,
            Description = createPost.Description,
            ReadingTime = createPost.ReadingTime,
            Image = createPost.Image,
            AddressId = createPost.AddressId
        };
        
        var user = await _tokenService.GetUserAsync();

        var tags = await GetTags(createPost.Tags);
        user.CreatedPosts.Add(post);
        post.Tags = tags;
        
        await _context.Posts.AddAsync(post);
        await _context.SaveChangesAsync();
        return new PostResponse { PostId = post.Id };
    }

    public async Task LikePostAsync(Guid id)
    {
        var user = await _tokenService.GetUserWithLikedPostsAsync();
        EnsureNoLikeExists(user, id);
        var post = await GetPostByIdAsync(id);
        ChangeLikeCounter(true, post);
        user.LikedPosts.Add(post);
        await _context.SaveChangesAsync();
    }

    public async Task UnlikePostAsync(Guid id)
    {
        var user = await _tokenService.GetUserWithLikedPostsAsync();
        EnsureLikeExists(user, id);
        var postToRemove = await GetPostByIdAsync(id);
        ChangeLikeCounter(false, postToRemove);
        user.LikedPosts.Remove(postToRemove);
        await _context.SaveChangesAsync();
    }

    private async Task CheckAddressExistence(Guid addressId)
    {
        if (!await _context.AddressElements.AnyAsync(a => a.ObjectGuid == addressId))
        {
            throw new AddressNotFoundException($"Not found address with ObjectGuid={addressId}");
        }
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
    
    private async Task<List<Tag>> GetTags(IEnumerable<Guid> tagsId)
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
}