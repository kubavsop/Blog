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
            _logger.LogWarning(exception, exception.Message);
            await SetExceptionAsync(context, StatusCodes.Status400BadRequest, exception.Message);
        }
        catch (UserNotFoundException exception)
        {
            _logger.LogWarning(exception, exception.Message);
            await SetExceptionAsync(context, StatusCodes.Status404NotFound, exception.Message);
        }
        catch (TagNotFoundException exception)
        {
            _logger.LogWarning(exception, exception.Message);
            await SetExceptionAsync(context, StatusCodes.Status404NotFound, exception.Message);
        }
        catch (LikeExistsException exception)
        {
            _logger.LogWarning(exception, exception.Message);
            await SetExceptionAsync(context, StatusCodes.Status400BadRequest, exception.Message);
        }
        catch (PostNotFoundException exception)
        {
            _logger.LogWarning(exception, exception.Message);
            await SetExceptionAsync(context, StatusCodes.Status404NotFound, exception.Message);
        }
        catch (CommentNotFoundException exception)
        {
            _logger.LogWarning(exception, exception.Message);
            await SetExceptionAsync(context, StatusCodes.Status404NotFound, exception.Message);
        }
        catch (ParentCommentDetachedFromPostException exception)
        {
            _logger.LogWarning(exception, exception.Message);
            await SetExceptionAsync(context, StatusCodes.Status400BadRequest, exception.Message);
        }
        catch (CommentOwnerMismatchException exception)
        {
            _logger.LogWarning(exception, exception.Message);
            await SetExceptionAsync(context, StatusCodes.Status403Forbidden, exception.Message);
        }
        catch (CommentDeletionException exception)
        {
            _logger.LogWarning(exception, exception.Message);
            await SetExceptionAsync(context, StatusCodes.Status400BadRequest, exception.Message);
        }
        catch (RootCommentException exception)
        {
            _logger.LogWarning(exception, exception.Message);
            await SetExceptionAsync(context, StatusCodes.Status400BadRequest, exception.Message);
        }
        catch (AddressNotFoundException exception)
        {
            _logger.LogWarning(exception, exception.Message);
            await SetExceptionAsync(context, StatusCodes.Status404NotFound, exception.Message);
        }
        catch (CommunityAlreadyExistsException exception)
        {
            _logger.LogWarning(exception, exception.Message);
            await SetExceptionAsync(context, StatusCodes.Status400BadRequest, exception.Message);
        }
        catch (TagAlreadyExistsException exception)
        {
            _logger.LogWarning(exception, exception.Message);
            await SetExceptionAsync(context, StatusCodes.Status400BadRequest, exception.Message);
        }
        catch (CommunityNotFoundException exception)
        {
            _logger.LogWarning(exception, exception.Message);
            await SetExceptionAsync(context, StatusCodes.Status404NotFound, exception.Message);
        }
        catch (UserRoleException exception)
        {
            _logger.LogWarning(exception, exception.Message);
            await SetExceptionAsync(context, StatusCodes.Status400BadRequest, exception.Message);
        }
        catch (CommunityAccessException exception)
        {
            _logger.LogWarning(exception, exception.Message);
            await SetExceptionAsync(context, StatusCodes.Status403Forbidden, exception.Message);
        }
        catch (UserNotAuthorizedException exception)
        {
            _logger.LogWarning(exception, exception.Message);
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        }
        catch (InvalidPageException exception)
        {
            _logger.LogWarning(exception, exception.Message);
            await SetExceptionAsync(context, StatusCodes.Status400BadRequest, exception.Message);
        }
        catch (NotAdminException exception)
        {
            _logger.LogWarning(exception, exception.Message);
            await SetExceptionAsync(context, StatusCodes.Status403Forbidden, exception.Message);
        }
        catch (InvalidCredentialsException exception)
        {
            _logger.LogWarning(exception, exception.Message);
            await SetExceptionAsync(context, StatusCodes.Status400BadRequest, exception.Message);
        }
        catch (RefreshTokenNotFoundException exception)
        {
            _logger.LogWarning(exception, exception.Message);
            await SetExceptionAsync(context, StatusCodes.Status404NotFound, exception.Message);
        }
        catch (RefreshTokenHasExpiredException exception)
        {
            _logger.LogWarning(exception, exception.Message);
            await SetExceptionAsync(context, StatusCodes.Status400BadRequest, exception.Message);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, exception.Message);
            await SetExceptionAsync(context, StatusCodes.Status500InternalServerError, exception.Message);
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
        public required string Message { get; set; }
    }
}