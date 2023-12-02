using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices.JavaScript;
using Blog.API.Common.Enums;
using Blog.API.Common.Mappers;
using Blog.API.Controllers.Dto.Requests;
using Blog.API.Controllers.Dto.Responses;
using Blog.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers;

[Authorize]
[Route("api/post")]
[ApiController]
public class PostController : ControllerBase
{
    private readonly IPostService _postService;

    public PostController(IPostService postService)
    {
        _postService = postService;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PostDto>>> GetPostsAsync(
        [FromQuery] IEnumerable<Guid> tags,
        string? author,
        [Range(0, int.MaxValue)] int? min,
        [Range(0, int.MaxValue)] int? max,
        PostSorting sorting,
        bool onlyMyCommunities = false,
        [Range(1, int.MaxValue)] int page = 1,
        [Range(1, int.MaxValue)] int size = 5
    )
    {
        var pagedList =
            await _postService.GetPostsAsync(tags, author, min, max, sorting, onlyMyCommunities, page, size);
        return Ok(PostMapper.PagedListPagedListDto(pagedList));
    }

    [HttpPost]
    public async Task<ActionResult<PostResponseDto>> CreatePostAsync(CreatePostDto createPostDto)
    {
        var postResponse = await _postService.CreatePostAsync(PostMapper.CreatePostDtoToCreatePost(createPostDto));
        return Ok(PostMapper.PostResponseToPostResponseDto(postResponse));
    }

    [AllowAnonymous]
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<PostFullDto>> GetInformationAboutPostAsync(Guid id)
    {
        var post = await _postService.GetInformationAboutPost(id);
        return Ok(PostMapper.PostFullToPostFullDto(post));
    }

    [HttpPost("{postId:guid}/like")]
    public async Task<ActionResult> LikePostAsync(Guid postId)
    {
        await _postService.LikePostAsync(postId);
        return Ok();
    }

    [HttpDelete("{postId:guid}/like")]
    public async Task<ActionResult> UnLikeAsync(Guid postId)
    {
        await _postService.UnlikePostAsync(postId);
        return Ok();
    }
}