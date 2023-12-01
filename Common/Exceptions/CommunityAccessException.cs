namespace Blog.API.Common.Exceptions;

public class CommunityAccessException: Exception
{
    public CommunityAccessException(string message) : base(message) { }
}