namespace Blog.API.Common.Exceptions;

public class RefreshTokenHasExpiredException: Exception
{
    public RefreshTokenHasExpiredException(string message) : base(message) { }
}