using Blog.API.Controllers.Dto.Requests;
using Blog.API.Controllers.Mappers;
using Blog.API.Entities;
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

    [Authorize]
    [HttpPut("comment/{id:guid}")]
    public async Task<ActionResult> EditCommentAsync([FromBody] UpdateCommentDto updateCommentDto, Guid id)
    {
        await _commentService.EditCommentAsync(CommentMapper.UpdateCommentDtoToUpdateComment(updateCommentDto), id);
        return Ok();
    }
}