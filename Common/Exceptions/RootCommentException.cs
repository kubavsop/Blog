namespace Blog.API.Common.Exceptions;

public class RootCommentException: Exception
{
    public RootCommentException(string message) : base(message) { }
}