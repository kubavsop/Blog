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
    }

    private static async Task SetExceptionAsync(HttpContext context, int status, string message)
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        await context.Response.WriteAsJsonAsync(new Error
        {
            StatusCode = StatusCodes.Status400BadRequest,
            Message = message
        });
    }
    
    private class Error
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
    }
}