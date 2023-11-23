using BCrypt.Net;
using Blog.API.Data;
using Blog.API.Entities;
using Blog.API.Middlewares.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Blog.API.Services.Impl;

public class UserService: IUserService
{
    private readonly AppDbContext _context;
    private readonly IJwtService _jwtService;

    public UserService(AppDbContext context, IJwtService jwtService)
    {
        _context = context;
        _jwtService = jwtService;
    }

    public async Task<TokenResponse> LoginUserAsync(LoginCredentials loginCredentials)
    {
        await CheckUserExistenceAsync(loginCredentials);
        return _jwtService.CreateToken(loginCredentials.Email);
    }
    
    public async Task<TokenResponse> CreateUserAsync(User user)
    {
        await CheckEmailExistenceAsync(user.Email);
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return _jwtService.CreateToken(user.Email);
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