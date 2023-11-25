using Blog.API.Data;
using Blog.API.Entities;
using Blog.API.Entities.Database;
using Blog.API.Middlewares.Exceptions;
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
        var post = new Post
        {
            Title = createPost.Title,
            Description = createPost.Description,
            ReadingTime = createPost.ReadingTime,
            Image = createPost.Image,
        };
        
        var user = await _tokenService.GetUserAsync();

        var tags = await GetTags(createPost.Tags);
        user.CreatedPosts.Add(post);
        post.Tags = tags;
        
        await _context.Posts.AddAsync(post);
        await _context.SaveChangesAsync();
        return new PostResponse { PostId = post.Id };
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