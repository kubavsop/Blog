namespace Blog.API.Common.Exceptions;

public class LikeExistsException: Exception
{
    public LikeExistsException(string message) : base(message) { }
}