using Blog.API.Data;
using Blog.API.Entities;
using Blog.API.Entities.Database;
using Blog.API.Middlewares.Exceptions;
using Microsoft.AspNetCore.Mvc;
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
        
        var author = await _context.Authors.FirstOrDefaultAsync(a => a.UserId == user.Id) ?? new Author
        {
            User = user
        };

        var tags = await GetTags(createPost.Tags);

        post.Author = author;
        post.Tags = tags;
        
        await _context.Posts.AddAsync(post);
        await _context.SaveChangesAsync();
        return new PostResponse { PostId = post.Id };
    }

    private async Task<List<Tag>> GetTags(IEnumerable<Guid> tagsId)
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
}