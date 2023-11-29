using Blog.API.Entities.Database;

namespace Blog.API.Services;

public interface IAddressService
{
    Task<IEnumerable<AddressElement>> SearchAddressAsync(long? parentObjectId, string? query);
    Task<IEnumerable<AddressElement>> GetAddressChainAsync(Guid objectGuid);
}