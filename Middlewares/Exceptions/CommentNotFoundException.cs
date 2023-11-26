namespace Blog.API.Middlewares.Exceptions;

public class CommentNotFoundException: Exception
{
    public CommentNotFoundException(string message) : base(message) { }
}