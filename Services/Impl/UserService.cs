using Blog.API.Common.Exceptions;
using Blog.API.Data;
using Blog.API.Entities;
using Blog.API.Entities.Database;
using Microsoft.EntityFrameworkCore;

namespace Blog.API.Services.Impl;

public class UserService : IUserService
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly ITokenProvider _tokenProvider;
    private readonly ITokenService _tokenService;

    public UserService(AppDbContext context, ITokenProvider tokenProvider, ITokenService tokenService, IConfiguration configuration)
    {
        _context = context;
        _tokenProvider = tokenProvider;
        _tokenService = tokenService;
        _configuration = configuration;
    }

    public async Task<TokenResponse> RefreshAsync(string token)
    {
        var refreshToken = await GetRefreshTokenAsync(token);
        if (refreshToken.IsExpired)
        {
            _context.RefreshTokens.Remove(refreshToken);
            await _context.SaveChangesAsync();
            throw new RefreshTokenHasExpiredException("Token has expired");
        }

        var tokenResponse = await CreateTokenResponseAsync(refreshToken.UserId);
        
        _context.RefreshTokens.Remove(refreshToken);
        await _context.SaveChangesAsync();

        return tokenResponse;
    }

    public async Task<User> GetUserAsync()
    {
        return await _tokenService.GetUserAsync();
    }

    public async Task EditUserAsync(UserEdit userEdit)
    {
        var user = await _tokenService.GetUserAsync();

        if (user.Email != userEdit.Email)
        {
            await CheckEmailExistenceAsync(userEdit.Email);
        }

        user.Email = userEdit.Email;
        user.PhoneNumber = userEdit.PhoneNumber;
        user.Gender = userEdit.Gender;
        user.FullName = userEdit.FullName;
        user.BirthDate = userEdit.BirthDate;
        await _context.SaveChangesAsync();
    }

    public async Task<TokenResponse> LoginUserAsync(LoginCredentials loginCredentials)
    {
        var id = await CheckUserExistenceAsync(loginCredentials);
        var tokenResponse = await CreateTokenResponseAsync(id);
        return tokenResponse;
    }

    public async Task<TokenResponse> CreateUserAsync(User user)
    {
        await CheckEmailExistenceAsync(user.Email);
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return await CreateTokenResponseAsync(user.Id);
    }

    public async Task LogoutUserAsync()
    {
        await _tokenService.InvalidateTokenAsync();
    }

    private async Task<TokenResponse> CreateTokenResponseAsync(Guid id)
    {
        var tokenResponse = _tokenProvider.CreateTokenResponse(id);
        var expireMinutes = _configuration.GetValue<double>("AppSettings:RefreshTokenExpireMinutes");
        
        await _context.RefreshTokens.AddAsync(new RefreshToken
        {
            ExpirationTime = DateTime.UtcNow.AddMinutes(expireMinutes),
            Token = tokenResponse.RefreshToken,
            UserId = id
        });
        await _context.SaveChangesAsync();
        
        return tokenResponse;
    }

    private async Task<RefreshToken> GetRefreshTokenAsync(string token)
    {
        var refreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(t => t.Token == token);

        if (refreshToken == null)
        {
            throw new RefreshTokenNotFoundException("Token not found in database");
        }

        return refreshToken;
    }

    private async Task<Guid> CheckUserExistenceAsync(LoginCredentials credentials)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == credentials.Email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(credentials.Password, user.Password))
        {
            throw new InvalidCredentialsException("Incorrect login or password");
        }

        return user.Id;
    }

    private async Task CheckEmailExistenceAsync(string email)
    {
        if (await _context.Users.AnyAsync(u => u.Email == email))
        {
            throw new UserAlreadyExistsException("User with this email already exists");
        }
    }
}