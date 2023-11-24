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

    public async Task<User> GetUserAsync()
    {
        var id = _tokenService.GetUserId();
        return await GetUserById(id);
    }

    public async Task EditUserAsync(UserEdit userEdit)
    {
        var id = _tokenService.GetUserId();
        var user = await GetUserById(id);
        
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
        return _tokenProvider.CreateToken(id);
    }
    
    public async Task<TokenResponse> CreateUserAsync(User user)
    {
        await CheckEmailExistenceAsync(user.Email);
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return _tokenProvider.CreateToken(user.Id);
    }

    public async Task LogoutUserAsync()
    {
        await _tokenService.InvalidateTokenAsync();
    }
    
    private async Task<Guid> CheckUserExistenceAsync(LoginCredentials credentials)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == credentials.Email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(credentials.Password, user.Password))
        {
            throw new UserNotFoundException("Incorrect login or password");
        }

        return user.Id;
    }
    
    private async Task<User> GetUserById(Guid id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
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