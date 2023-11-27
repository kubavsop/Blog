using Blog.API.Middlewares.Exceptions;

namespace Blog.API.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (UserAlreadyExistsException exception)
        {
            await SetExceptionAsync(context, StatusCodes.Status400BadRequest, exception.Message);
        }
        catch (UserNotFoundException exception)
        {
            await SetExceptionAsync(context, StatusCodes.Status400BadRequest, exception.Message);
        }
        catch (TagNotFoundException exception)
        {
            await SetExceptionAsync(context, StatusCodes.Status404NotFound, exception.Message);
        }
        catch (LikeExistsException exception)
        {
            await SetExceptionAsync(context, StatusCodes.Status400BadRequest, exception.Message);
        }
        catch (PostNotFoundException exception)
        {
            await SetExceptionAsync(context, StatusCodes.Status404NotFound, exception.Message);
        }
        catch (CommentNotFoundException exception)
        {
            await SetExceptionAsync(context, StatusCodes.Status404NotFound, exception.Message);
        }
        catch (ParentCommentDetachedFromPostException exception)
        {
            await SetExceptionAsync(context, StatusCodes.Status400BadRequest, exception.Message);
        }
        catch (CommentOwnerMismatchException exception)
        {
            await SetExceptionAsync(context, StatusCodes.Status403Forbidden, exception.Message);
        }
        catch (CommentDeletionException exception)
        {
            await SetExceptionAsync(context, StatusCodes.Status400BadRequest, exception.Message);
        }
        catch (RootCommentException exception)
        {
            await SetExceptionAsync(context, StatusCodes.Status400BadRequest, exception.Message);
        }
    }

    private static async Task SetExceptionAsync(HttpContext context, int status, string message)
    {
        context.Response.StatusCode = status;
        await context.Response.WriteAsJsonAsync(new Error
        {
            StatusCode = status,
            Message = message
        });
    }
    
    private class Error
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
    }
}