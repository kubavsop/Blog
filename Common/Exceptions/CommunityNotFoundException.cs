namespace Blog.API.Common.Exceptions;

public class CommunityNotFoundException: Exception
{
    public CommunityNotFoundException(string message) : base(message) { }
}