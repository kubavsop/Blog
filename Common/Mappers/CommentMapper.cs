using Blog.API.Controllers.Dto.Requests;
using Blog.API.Controllers.Dto.Responses;
using Blog.API.Entities;
using Blog.API.Entities.Database;

namespace Blog.API.Common.Mappers;

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

    public static UpdateComment UpdateCommentDtoToUpdateComment(UpdateCommentDto updateComment)
    {
        return new UpdateComment
        {
            Content = updateComment.Content
        };
    }

    public static IEnumerable<CommentDto> CommentsToCommentsDto(IEnumerable<Comment> comments)
    {
        return comments.Select(CommentToCommentDto);
    }

    private static CommentDto CommentToCommentDto(Comment comment)
    {
        return new CommentDto
        {
            Id = comment.Id,
            CreateTime = comment.CreateTime,
            Content = comment.Content,
            ModifiedDate = comment.ModifiedDate,
            DeleteDate = comment.DeleteDate,
            AuthorId = comment.AuthorId,
            Author = comment.Author.FullName,
            SubComments = comment.SubComments
        };
    }
}