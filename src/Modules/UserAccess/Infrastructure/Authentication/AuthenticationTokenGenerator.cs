using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using App.BuildingBlocks.Infrastructure.Authentication;
using App.Modules.UserAccess.Application.Authentication;
using Microsoft.IdentityModel.Tokens;

namespace App.Modules.UserAccess.Infrastructure.Authentication;

public class AuthenticationTokenGenerator : IAuthenticationTokenGenerator
{
    private readonly AuthenticationConfiguration _configuration;

    public AuthenticationTokenGenerator(IAuthenticationConfigurationProvider configurationProvider)
    {
        _configuration = configurationProvider.GetConfiguration();
    }

    public Token GenerateAccessToken(Guid userId)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.IssuerSigningKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString())
        };

        var expires = DateTime.UtcNow.AddSeconds(_configuration.AccessTokenTtl);
        var token = new JwtSecurityToken(
            _configuration.Issuer,
            _configuration.Audience,
            claims,
            expires: expires,
            signingCredentials: credentials);

        return new Token(new JwtSecurityTokenHandler().WriteToken(token), expires);
    }

    public Token GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var randomNumberGenerator = RandomNumberGenerator.Create();
        randomNumberGenerator.GetBytes(randomNumber);

        return new Token(Convert.ToBase64String(randomNumber), DateTime.UtcNow.AddSeconds(_configuration.RefreshTokenTtl));
    }

    public Token GenerateRefreshToken(string value)
    {
        return new Token(value, DateTime.UtcNow.AddSeconds(_configuration.RefreshTokenTtl));
    }
}
