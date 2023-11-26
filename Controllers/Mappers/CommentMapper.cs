using Blog.API.Controllers.Dto.Requests;
using Blog.API.Entities;

namespace Blog.API.Controllers.Mappers;

internal static class CommentMapper
{
    public static CreateComment CreateCommentDtoToCreateComment(CreateCommentDto commentDto)
    {
        return new CreateComment
        {
            Content = commentDto.Content,
            ParentId = commentDto.ParentId
        };
    }
}