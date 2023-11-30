using Blog.API.Common.Mappers;
using Blog.API.Controllers.Dto.Requests;
using Blog.API.Controllers.Dto.Responses;
using Blog.API.Entities;
using Blog.API.Services;
using Microsoft.AspNetCore.Authorization;
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

    [HttpPost]
    [Authorize]
    public async Task<ActionResult> CreateCommunityAsync(CreateCommunityDto communityDto)
    {
        var community = CommunityMapper.CreateCommunityDtoToCreateCommunity(communityDto);
        await _communityService.CreateCommunityAsync(community);
        return Ok();
    }

    [Authorize]
    [HttpGet("my")]
    public async Task<ActionResult<IEnumerable<CommunityUserDto>>> GetUserCommunitiesAsync()
    {
        var communities = await _communityService.GetUserCommunitiesAsync();
        return Ok(CommunityMapper.CommunitiesUserToCommunitiesUserDto(communities));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CommunityFullDto>> GetInformationAboutCommunityAsync(Guid id)
    {
        var community = await _communityService.GetInformationAboutCommunityAsync(id);
        return Ok(CommunityMapper.CommunityFullToCommunityFullDto(community));
    }

    [Authorize]
    [HttpGet("{id:guid}/role")]
    public async Task<ActionResult<RoleResponseDto>> GetUserRoleAsync(Guid id)
    {
        var role = await _communityService.GetUserRoleAsync(id);
        return Ok(CommunityMapper.RoleResponseToRoleResponseDto(role));
    }

    [Authorize]
    [HttpPost("{id:guid}/subscribe")]
    public async Task<ActionResult> SubscribeUserToCommunityAsync(Guid id)
    {
        await _communityService.SubscribeUserToCommunityAsync(id);
        return Ok();
    }

    [Authorize]
    [HttpDelete("{id:guid}/unsubscribe")]
    public async Task<ActionResult> UnsubscribeUserToCommunityAsync(Guid id)
    {
        await _communityService.UnsubscribeUserToCommunityAsync(id);
        return Ok();
    }
}