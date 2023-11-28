namespace Blog.API.Common.Exceptions;

public class ParentCommentDetachedFromPostException: Exception
{
    public ParentCommentDetachedFromPostException(string message) : base(message) { }
}