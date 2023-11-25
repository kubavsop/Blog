using Blog.API.Controllers.Dto.Requests;
using Blog.API.Controllers.Dto.Responses;
using Blog.API.Controllers.Mappers;
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
    
}