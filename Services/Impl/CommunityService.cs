using Blog.API.Data;
using Blog.API.Entities.Database;
using Microsoft.EntityFrameworkCore;

namespace Blog.API.Services.Impl;

public class CommunityService: ICommunityService
{
    private readonly AppDbContext _context;

    public CommunityService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Community>> GetCommunityListAsync()
    {
        return await _context.Communities.ToListAsync();
    }
}