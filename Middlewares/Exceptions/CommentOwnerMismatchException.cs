namespace Blog.API.Middlewares.Exceptions;

public class CommentOwnerMismatchException: Exception
{
    public CommentOwnerMismatchException(string message) : base(message) { }
}