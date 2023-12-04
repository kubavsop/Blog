using Blog.API.Common.Exceptions;

namespace Blog.API.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (UserAlreadyExistsException exception)
        {
            _logger.LogError(exception, exception.Message);
            await SetExceptionAsync(context, StatusCodes.Status400BadRequest, exception.Message);
        }
        catch (UserNotFoundException exception)
        {
            await SetExceptionAsync(context, StatusCodes.Status404NotFound, exception.Message);
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
        catch (AddressNotFoundException exception)
        {
            await SetExceptionAsync(context, StatusCodes.Status404NotFound, exception.Message);
        }
        catch (CommunityAlreadyExistsException exception)
        {
            await SetExceptionAsync(context, StatusCodes.Status400BadRequest, exception.Message);
        }
        catch (TagAlreadyExistsException exception)
        {
            await SetExceptionAsync(context, StatusCodes.Status400BadRequest, exception.Message);
        }
        catch (CommunityNotFoundException exception)
        {
            await SetExceptionAsync(context, StatusCodes.Status404NotFound, exception.Message);
        }
        catch (UserRoleException exception)
        {
            await SetExceptionAsync(context, StatusCodes.Status400BadRequest, exception.Message);
        }
        catch (CommunityAccessException exception)
        {
            await SetExceptionAsync(context, StatusCodes.Status403Forbidden, exception.Message);
        }
        catch (UserNotAuthorizedException exception)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        }
        catch (InvalidPageException exception)
        {
            await SetExceptionAsync(context, StatusCodes.Status400BadRequest, exception.Message);
        }
        catch (NotAdminException exception)
        {
            await SetExceptionAsync(context, StatusCodes.Status403Forbidden, exception.Message);
        }
        catch (InvalidCredentialsException exception)
        {
            await SetExceptionAsync(context, StatusCodes.Status400BadRequest, exception.Message);
        }
        catch (Exception exception)
        {
            await SetExceptionAsync(context, StatusCodes.Status500InternalServerError, exception.Message);
        }
    }

    private static async Task SetExceptionAsync(HttpContext context, int status, string message)
    {
        context.Response.StatusCode = status;
        await context.Response.WriteAsJsonAsync(new Error
        {
            Status = "Error",
            Message = message
        });
    }

    private class Error
    {
        public string Status { get; set; }
        public string Message { get; set; }
    }
}