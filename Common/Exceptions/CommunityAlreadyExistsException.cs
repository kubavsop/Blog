namespace Blog.API.Common.Exceptions;

public class CommunityAlreadyExistsException: Exception
{
    public CommunityAlreadyExistsException(string message) : base(message) { }
}