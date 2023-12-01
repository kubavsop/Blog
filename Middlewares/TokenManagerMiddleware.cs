using Blog.API.Services;
using Microsoft.AspNetCore.Authorization;

namespace Blog.API.Middlewares;

public class TokenManagerMiddleware
{

    private readonly RequestDelegate _next;

    public TokenManagerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ITokenService tokenService)
    {
        var hasAuthorizeAttribute = context.GetEndpoint()?.Metadata.GetMetadata<AuthorizeAttribute>() == null;
        var hasAllowAnonymousAttribute = context.GetEndpoint()?.Metadata.GetMetadata<AllowAnonymousAttribute>() != null;
        
        if (hasAllowAnonymousAttribute || hasAuthorizeAttribute || !await tokenService.CheckTokenAsync())
        {
            await _next(context);
            return;
        }

        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
    }
}