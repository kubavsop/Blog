using Blog.API.Controllers.Dto.Requests;
using Blog.API.Controllers.Dto.Responses;
using Blog.API.Entities;

namespace Blog.API.Controllers.Mappers;

internal static class UserMapper
{
    public static User UserDtoToUser(UserRegisterDto user)
    {
        return new User
        {
            FullName = user.FullName,
            Password = BCrypt.Net.BCrypt.HashPassword(user.Password),
            BirthDate = user.BirthDate,
            Gender = user.Gender,
            PhoneNumber = user.PhoneNumber,
            Email = user.Email
        };
    }

    public static LoginCredentials LoginCredentialsDtoToLoginCredentials(LoginCredentialsDto credentials)
    {
        return new LoginCredentials
        {
            Email = credentials.Email,
            Password = credentials.Password
        };
    }
    public static TokenResponseDto TokenResponseToTokenResponseDto(TokenResponse tokenResponse)
    {
        return new TokenResponseDto
        {
            Token = tokenResponse.Token
        };
    }
}