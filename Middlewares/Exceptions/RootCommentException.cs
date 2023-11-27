namespace Blog.API.Middlewares.Exceptions;

public class RootCommentException: Exception
{
    public RootCommentException(string message) : base(message) { }
}