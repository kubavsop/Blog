using Blog.API.Controllers.Dto.Requests;
using Blog.API.Controllers.Dto.Responses;
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

    [HttpGet("comment/{id:guid}/tree")]
    public async Task<ActionResult<IEnumerable<CommentDto>>> GetCommentsAsync(Guid id)
    {
        var comments = await _commentService.GetCommentsAsync(id);
        return Ok(CommentMapper.CommentsToCommentsDto(comments));
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

    [Authorize]
    [HttpDelete("comment/{id:guid}")]
    public async Task<ActionResult> DeleteCommentAsync(Guid id)
    {
        await _commentService.DeleteCommentAsync(id);
        return Ok();
    }

    
}