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
        var comment = await GetCommentAsync(commentId);
        
        if (comment.ParentId != null)
        {
            throw new RootCommentException($"Comment with id={commentId} is not a root element");
        }

        var comments = new LinkedList<Comment>();
        
        await GetCommentTreeAsync(comments, comment);
        
        return comments;
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
        var comment = await GetCommentAsync(commentId);

        if (comment.DeleteDate != null)
        {
            throw new CommentDeletionException($"Comment with id={commentId} is deleted");
        }
        
        CheckAuthor(comment.AuthorId);
        
        comment.Content = updateComment.Content;
        comment.ModifiedDate = DateTime.UtcNow;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteCommentAsync(Guid commentId)
    {
        var comment = await GetCommentAsync(commentId);
        
        CheckAuthor(comment.AuthorId);

        if (comment.DeleteDate != null)
        {
            throw new CommentDeletionException($"This comment with id={commentId} has already been deleted");
        }
        
        comment.ModifiedDate = DateTime.UtcNow;
        comment.DeleteDate = DateTime.UtcNow;
        comment.Content = string.Empty;

        if (comment.SubComments == 0)
        {
            var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == comment.PostId);
            _context.Comments.Remove(comment);
            post!.CommentsCount -= 1;
            
            if (comment.ParentId != null)
            {
                var parentComment = await GetCommentAsync(comment.ParentId.GetValueOrDefault());
                parentComment.SubComments -= 1;
            }
        }

        await _context.SaveChangesAsync();
    }

    private async Task GetCommentTreeAsync(LinkedList<Comment> comments, Comment rootComment)
    {
        var stack = new Stack<Comment>();

        await GetSubComments(stack, rootComment);

        while (stack.Count > 0)
        {
            var currentComment = stack.Pop();
            
            comments.AddLast(currentComment);

            await GetSubComments(stack, currentComment);
        }
    }

    private async Task GetSubComments(Stack<Comment> stack, Comment comment)
    {
        
        var subComments = await _context.Comments
            .Include(c => c.Author)
            .Where(c => c.ParentId == comment.Id)
            .OrderByDescending(c => c.CreateTime)
            .ToListAsync();

        foreach (var subComment in subComments)
        {
            stack.Push(subComment);
        }
    }

    private void CheckAuthor(Guid authorId)
    {
        var userId = _tokenService.GetUserId();
        if (userId != authorId)
        {
            throw new CommentOwnerMismatchException($"The user with id={userId} is not the author of the comment");
        }
    }

    private async Task<Comment> GetCommentAsync(Guid commentId)
    {
        var comment = await _context.Comments
            .FirstOrDefaultAsync(c => c.Id == commentId);
        
        if (comment == null)
        {
            throw new CommentNotFoundException(
                $"Comment with id={commentId} not found in database");
        }

        return comment;
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
                $"Comment with id={parentCommentId} not found in database");
        }

        if (parentComment.PostId != id)
        {
            throw new ParentCommentDetachedFromPostException("Parent comment does not belong to post");
        }

        parentComment.SubComments += 1;

        return post;
    }
}