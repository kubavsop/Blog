namespace Blog.API.Common.Exceptions;

public class InvalidPageException: Exception
{
    public InvalidPageException(string message) : base(message) { }
}