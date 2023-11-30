using Blog.API.Common.Mappers;
using Blog.API.Controllers.Dto.Requests;
using Blog.API.Controllers.Dto.Responses;
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

    [HttpPost]
    public async Task<ActionResult> CreateTagAsync(CreateTag createTag)
    {
        var tag = TagMapper.CreateTagDtoToTag(createTag);
        await _tagService.CreateTagAsync(tag);
        return Ok();
    }
}