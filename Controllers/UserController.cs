using Blog.API.Controllers.Dto.Requests;
using Blog.API.Controllers.Dto.Responses;
using Blog.API.Controllers.Mappers;
using Blog.API.Entities;
using Blog.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers;


[Route("api/account")]
[ApiController]
public class UserController: ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService service)
    {
        _userService = service;
    }
    
    [HttpPost("register")]
    public async Task<ActionResult<TokenResponseDto>> RegisterAsync(UserRegisterDto user)
    {
        var tokenResponseDto 
            = UserMapper.TokenResponseToTokenResponseDto(await _userService.CreateUserAsync(UserMapper.UserDtoToUser(user)));
        return Ok(tokenResponseDto);
    }

    [HttpPost("login")]
    public async Task<ActionResult<TokenResponseDto>> LoginAsync(LoginCredentialsDto credentialsDto)
    {
        var credentials = UserMapper.LoginCredentialsDtoToLoginCredentials(credentialsDto);
        var tokenResponseDto
            = UserMapper.TokenResponseToTokenResponseDto(await _userService.LoginUserAsync(credentials));
        return Ok(tokenResponseDto);
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<ActionResult> LogoutAsync()
    {
        await _userService.LogoutUserAsync();
        return Ok();
    }
}