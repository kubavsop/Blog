using Blog.API.Data;
using Blog.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Blog.API.Services.Impl;

public class AuthorService: IAuthorService
{
    private readonly AppDbContext _context;

    public AuthorService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Author>> GetAuthorsAsync()
    {
        return await _context.Users
            .Where(u => u.CreatedPosts.Count != 0)
            .Include(user => user.CreatedPosts)
            .Select(user => new Author
            {
                FullName = user.FullName,
                BirthDate = user.BirthDate,
                Gender = user.Gender,
                Posts = user.CreatedPosts.Count,
                Likes = user.Likes,
                Created = user.CreateTime
            })
            .ToListAsync();
    }
}