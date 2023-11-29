using Blog.API.Common.Mappers;
using Blog.API.Controllers.Dto.Responses;
using Blog.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers;

[Route("api/community")]
[ApiController]
public class CommunityController : ControllerBase
{
    private readonly ICommunityService _communityService;

    public CommunityController(ICommunityService communityService)
    {
        _communityService = communityService;
    }

    [HttpGet]
    public async Task<ActionResult<CommunityDto>> GetCommunityListAsync()
    {
        var communities = await _communityService.GetCommunityListAsync();
        return Ok(CommunityMapper.CommunitiesToCommunitiesDto(communities));
    }
}