namespace Blog.API.Common.Exceptions;

public class CommentOwnerMismatchException: Exception
{
    public CommentOwnerMismatchException(string message) : base(message) { }
}