using Blog.API.Controllers.Dto.Requests;
using Blog.API.Controllers.Mappers;
using Blog.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers;

[Route("api/")]
[ApiController]
public class CommentController : ControllerBase
{
    private readonly ICommentService _commentService;

    public CommentController(ICommentService commentService)
    {
        _commentService = commentService;
    }

    [Authorize]
    [HttpPost("post/{id:guid}/comment")]
    public async Task<ActionResult> AddCommentAsync([FromBody] CreateCommentDto commentDto, Guid id)
    {
        await _commentService.AddCommentAsync(CommentMapper.CreateCommentDtoToCreateComment(commentDto), id);
        return Ok();
    }
}