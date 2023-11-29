using Blog.API.Common.Mappers;
using Blog.API.Controllers.Dto.Requests;
using Blog.API.Controllers.Dto.Responses;
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
            = UserMapper.TokenResponseToTokenResponseDto(await _userService.CreateUserAsync(UserMapper.UserRegisterDtoToUser(user)));
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

    [Authorize]
    [HttpGet("profile")]
    public async Task<ActionResult<UserDto>> GetProfileAsync()
    {
        var user = await _userService.GetUserAsync();
        return Ok(UserMapper.UserToUserDto(user));
    }

    [Authorize]
    [HttpPut("profile")]
    public async Task<ActionResult> EditProfileAsync(UserEditDto userEditDto)
    {
        await _userService.EditUserAsync(UserMapper.UserEditDtoToUserEdit(userEditDto));
        return Ok();
    }
}