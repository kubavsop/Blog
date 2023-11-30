using Blog.API.Common.Exceptions;
using Blog.API.Data;
using Blog.API.Entities.Database;
using Microsoft.EntityFrameworkCore;

namespace Blog.API.Services.Impl;

public class TagService: ITagService
{
    private readonly AppDbContext _context;

    public TagService(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<Tag>> GetTagsAsync()
    {
        return await _context.Tags.ToListAsync();
    }

    public async Task CreateTagAsync(Tag tag)
    {
        await CheckNameExistenceAsync(tag.Name);
        await _context.Tags.AddAsync(tag);
        await _context.SaveChangesAsync();
    }
    
    private async Task CheckNameExistenceAsync(string name)
    {
        if (await _context.Tags.AnyAsync(t => t.Name == name))
        {
            throw new TagAlreadyExistsException("Tag with this name already exists");
        }
    }
}