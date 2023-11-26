using Blog.API.Data;
using Blog.API.Entities;
using Blog.API.Entities.Database;
using Blog.API.Middlewares.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Blog.API.Services.Impl;

public class CommentService : ICommentService
{
    private readonly AppDbContext _context;
    private readonly ITokenService _tokenService;

    public CommentService(AppDbContext context, ITokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    public async Task<IEnumerable<Comment>> GetCommentsAsync(Guid commentId)
    {
        throw new NotImplementedException();
    }

    public async Task AddCommentAsync(CreateComment comment, Guid postId)
    {
        // TODO Community work
        var user = await _tokenService.GetUserAsync();
        var post = await GetPostByIdAsyncWithCheck(postId, comment.ParentId);
        var newComment = new Comment
        {
            ParentId = comment.ParentId,
            Content = comment.Content,
        };

        user.Comments.Add(newComment);
        post.Comments.Add(newComment);
        post.CommentsCount += 1;

        await _context.SaveChangesAsync();
    }

    public async Task EditCommentAsync(UpdateComment updateComment, Guid commentId)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteCommentAsync(Guid commentId)
    {
        throw new NotImplementedException();
    }


    private async Task<Post> GetPostByIdAsyncWithCheck(Guid id, Guid? parentCommentId)
    {
        var post = await _context.Posts
            .FirstOrDefaultAsync(p => p.Id == id);

        if (post == null)
        {
            throw new PostNotFoundException($"Post with id={id} not found in database");
        }

        if (parentCommentId == null) return post;

        var parentComment = await _context.Comments
            .FirstOrDefaultAsync(c => c.Id == parentCommentId);

        if (parentComment == null)
        {
            throw new CommentNotFoundException(
                $"Comment with id={parentCommentId} not found in  database");
        }

        if (parentComment.PostId != id)
        {
            throw new ParentCommentDetachedFromPostException("Parent comment does not belong to post");
        }

        parentComment.SubComments += 1;

        return post;
    }
}