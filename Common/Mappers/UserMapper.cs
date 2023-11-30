using Blog.API.Controllers.Dto.Requests;
using Blog.API.Controllers.Dto.Responses;
using Blog.API.Entities;
using Blog.API.Entities.Database;

namespace Blog.API.Common.Mappers;

internal static class UserMapper
{
    public static User UserRegisterDtoToUser(UserRegisterDto user)
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

    public static UserDto UserToUserDto(User user)
    {
        return new UserDto
        {
            Id = user.Id,
            BirthDate = user.BirthDate,
            Email = user.Email,
            CreateTime = user.CreateTime,
            FullName = user.FullName,
            Gender = user.Gender,
            PhoneNumber = user.PhoneNumber
        };
    }

    public static UserEdit UserEditDtoToUserEdit(UserEditDto userEdit)
    {
        return new UserEdit
        {
            Email = userEdit.Email,
            BirthDate = userEdit.BirthDate,
            FullName = userEdit.FullName,
            Gender = userEdit.Gender,
            PhoneNumber = userEdit.PhoneNumber
        };
    }

    public static IEnumerable<UserDto> UsersToUsersDto(IEnumerable<User> users)
    {
        return users.Select(UserToUserDto);
    }
}