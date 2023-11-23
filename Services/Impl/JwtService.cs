using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Blog.API.Entities;
using Microsoft.IdentityModel.Tokens;

namespace Blog.API.Services.Impl;

public class JwtService: IJwtService
{
    private readonly IConfiguration _configuration;

    public JwtService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public TokenResponse CreateToken(string email) {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("AppSettings:SecretKey")!);
        var expireMinutes = _configuration.GetValue<double>("AppSettings:TokenExpireMinutes");
        var issuer = _configuration.GetValue<string>("AppSettings:Issuer")!;
        var tokenDescriptor = new SecurityTokenDescriptor
        
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new (ClaimTypes.Email, email),
            }),
            Expires = DateTime.UtcNow.AddMinutes(expireMinutes),
            Issuer = issuer,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return new TokenResponse { Token = tokenHandler.WriteToken(token) };
    }
}