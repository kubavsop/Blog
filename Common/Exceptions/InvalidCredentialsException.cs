namespace Blog.API.Common.Exceptions;

public class InvalidCredentialsException: Exception
{
    public InvalidCredentialsException(string message) : base(message) { }
    
}