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
public class PostController: ControllerBase
{
    private readonly IPostService _postService;

    public PostController(IPostService postService)
    {
        _postService = postService;
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