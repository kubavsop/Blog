namespace Blog.API.Common.Exceptions;

public class CommentDeletionException: Exception
{
    public CommentDeletionException(string message) : base(message) { }
}