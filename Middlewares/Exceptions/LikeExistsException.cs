namespace Blog.API.Middlewares.Exceptions;

public class LikeExistsException: Exception
{
    public LikeExistsException(string message) : base(message) { }
}