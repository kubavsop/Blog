namespace Blog.API.Common.Exceptions;

public class NotAdminException: Exception
{
    public NotAdminException(string message) : base(message) { }
}