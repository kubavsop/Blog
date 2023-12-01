namespace Blog.API.Common.Exceptions;

public class UserNotAuthorizedException: Exception
{
    public UserNotAuthorizedException(string? message = null) : base(message) { }
}