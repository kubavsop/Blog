using Blog.API.Data;
using Blog.API.Entities;
using Blog.API.Middlewares.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Blog.API.Services.Impl;

public class UserService: IUserService
{
    private readonly AppDbContext _context;
    private readonly ITokenProvider _tokenProvider;
    private readonly ITokenService _tokenService;

    public UserService(AppDbContext context, ITokenProvider tokenProvider, ITokenService tokenService)
    {
        _context = context;
        _tokenProvider = tokenProvider;
        _tokenService = tokenService;
    }

    public async Task<TokenResponse> LoginUserAsync(LoginCredentials loginCredentials)
    {
        await CheckUserExistenceAsync(loginCredentials);
        return _tokenProvider.CreateToken(loginCredentials.Email);
    }
    
    public async Task<TokenResponse> CreateUserAsync(User user)
    {
        await CheckEmailExistenceAsync(user.Email);
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return _tokenProvider.CreateToken(user.Email);
    }

    public async Task LogoutUserAsync()
    {
        await _tokenService.InvalidateTokenAsync();
    }
    
    private async Task CheckUserExistenceAsync(LoginCredentials credentials)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == credentials.Email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(credentials.Password, user.Password))
        {
            throw new UserNotFoundException("Incorrect login or password");
        }
    }
    
    private async Task<User> GetUserByEmail(string email)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null)
        {
            throw new UserNotFoundException("User not found");
        }
        return user;
    }

    private async Task CheckEmailExistenceAsync(string email)
    {
        if (await _context.Users.AnyAsync(u => u.Email == email))
        {
            throw new UserAlreadyExistsException("User with this email already exists");
        }
    }
}