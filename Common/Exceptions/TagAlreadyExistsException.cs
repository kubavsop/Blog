namespace Blog.API.Common.Exceptions;

public class TagAlreadyExistsException: Exception
{
    public TagAlreadyExistsException(string message) : base(message) { }
}