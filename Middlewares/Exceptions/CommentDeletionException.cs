namespace Blog.API.Middlewares.Exceptions;

public class CommentDeletionException: Exception
{
    public CommentDeletionException(string message) : base(message) { }
}