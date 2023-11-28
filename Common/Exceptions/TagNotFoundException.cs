namespace Blog.API.Common.Exceptions;

public class TagNotFoundException: Exception
{
    public TagNotFoundException(string message) : base(message) { }
}