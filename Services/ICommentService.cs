using Blog.API.Entities;
using Blog.API.Entities.Database;

namespace Blog.API.Services;

public interface ICommentService
{
    Task<IEnumerable<Comment>> GetCommentsAsync(Guid commentId);
    Task AddCommentAsync(CreateComment comment, Guid postId);
    Task EditCommentAsync(UpdateComment updateComment, Guid commentId);
    Task DeleteCommentAsync(Guid commentId);
}