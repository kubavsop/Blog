using Blog.API.Common.Exceptions;
using Blog.API.Data;
using Blog.API.Entities.Database;
using Microsoft.EntityFrameworkCore;

namespace Blog.API.Services.Impl;

public class AddressService : IAddressService
{
    private readonly AppDbContext _context;

    public AddressService(AppDbContext context)
    {
        _context = context;
    }


    public async Task<IEnumerable<AddressElement>> SearchAddressAsync(long? parentObjectId, string? query)
    {
        parentObjectId ??= 0;

        if (query is null or "")
        {
            return await _context.AddressElements
                .Where(a => a.ParentObjId == parentObjectId)
                .Take(10)
                .ToListAsync();
        }

        var normalizedQuery = query.Replace(" ", "").ToLower();

        return await _context.AddressElements
            .Where(a => a.ParentObjId == parentObjectId && a.NormalizedText.Contains(normalizedQuery))
            .ToListAsync();
    }

    public async Task<IEnumerable<AddressElement>> GetAddressChainAsync(Guid objectGuid)
    {
        var parentObject = await _context.AddressElements.FirstOrDefaultAsync(a => a.ObjectGuid == objectGuid);

        if (parentObject == null)
        {
            throw new AddressNotFoundException($"Not found object with ObjectGuid={objectGuid}");
        }

        var addressChain = new LinkedList<AddressElement>();

        while (parentObject != null)
        {
            addressChain.AddFirst(parentObject);
            parentObject =
                await _context.AddressElements.FirstOrDefaultAsync(a => a.ObjectId == parentObject.ParentObjId);
        }

        return addressChain;
    }
}