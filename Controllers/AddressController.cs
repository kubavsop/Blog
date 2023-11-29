using Blog.API.Common.Mappers;
using Blog.API.Controllers.Dto.Responses;
using Blog.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers;

[Route("api/address")]
[ApiController]
public class AddressController: ControllerBase
{
    private readonly IAddressService _addressService;

    public AddressController(IAddressService addressService)
    {
        _addressService = addressService;
    }

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<SearchAddressDto>>> SearchAddressAsync(long? parentObjectId, string? query)
    {
        var addresses = await _addressService.SearchAddressAsync(parentObjectId, query);
        return Ok(AddressMapper.AddressesToAddressesDto(addresses));
    }

    [HttpGet("chain")]
    public async Task<ActionResult<IEnumerable<SearchAddressDto>>> GetAddressChainAsync(Guid objectGuid)
    {
        var addresses = await _addressService.GetAddressChainAsync(objectGuid);
        return Ok(AddressMapper.AddressesToAddressesDto(addresses));
    }
}