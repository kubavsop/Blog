namespace Blog.API.Middlewares.Exceptions;

public class ParentCommentDetachedFromPostException: Exception
{
    public ParentCommentDetachedFromPostException(string message) : base(message) { }
}