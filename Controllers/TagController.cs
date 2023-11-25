using Blog.API.Controllers.Dto.Responses;
using Blog.API.Controllers.Mappers;
using Blog.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers;

[Route("api/tag")]
[ApiController]
public class TagController: ControllerBase
{
    private readonly ITagService _tagService;

    public TagController(ITagService tagService)
    {
        _tagService = tagService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TagDto>>> GetTagsAsync()
    {
        var tags = await _tagService.GetTagsAsync();
        return Ok(TagMapper.TagsToTagsDto(tags));
    }
}