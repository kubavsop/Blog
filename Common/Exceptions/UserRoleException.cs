namespace Blog.API.Common.Exceptions;

public class UserRoleException: Exception
{
    public UserRoleException(string message) : base(message) { }
}