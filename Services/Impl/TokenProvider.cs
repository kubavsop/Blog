using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Blog.API.Entities;
using Microsoft.IdentityModel.Tokens;

namespace Blog.API.Services.Impl;

public class TokenProvider : ITokenProvider
{
    private readonly IConfiguration _configuration;

    public TokenProvider(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public TokenResponse CreateTokenResponse(Guid id)
    {
        return new TokenResponse
        {
            AccessToken = CreateAccessToken(id),
            RefreshToken = CreateRefreshToken()
        };
    }

    private string CreateAccessToken(Guid id)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("AppSettings:SecretKey")!);
        var expireMinutes = _configuration.GetValue<double>("AppSettings:AccessTokenExpireMinutes");
        var issuer = _configuration.GetValue<string>("AppSettings:Issuer")!;
        var tokenId = Guid.NewGuid();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new(ClaimTypes.Name, id.ToString()),
                new(ClaimTypes.NameIdentifier, tokenId.ToString())
            }),
            Expires = DateTime.UtcNow.AddMinutes(expireMinutes),
            Issuer = issuer,
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    private string CreateRefreshToken()
    {
        return GenerateRandomString();
    }

    private static string GenerateRandomString()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }
}