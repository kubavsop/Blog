namespace Blog.API.Common.Exceptions;

public class AddressNotFoundException: Exception
{
    public AddressNotFoundException(string message) : base(message) { }
}