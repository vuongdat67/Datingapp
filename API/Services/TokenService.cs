using API.Entities;
using API.interfaces;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace API.Services;

public class TokenService(IConfiguration configuration) : ITokenService
{
    public string CreateToken(AppUser user)
    {
        var tokenKey = configuration["TokenKey"] ?? throw new ArgumentNullException("TokenKey is not configured.");
        if (tokenKey.Length < 64)
        {
            throw new ArgumentException("Token key must be at least 64 characters long.");
        }
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };
        // var token = new JwtSecurityToken(
        //     issuer: configuration["TokenIssuer"],
        //     audience: configuration["TokenAudience"],
        //     claims: claims,
        //     expires: DateTime.UtcNow.AddDays(7),
        //     signingCredentials: creds
        // );
        // return new JwtSecurityTokenHandler().WriteToken(token);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = creds,
            Issuer = configuration["TokenIssuer"],
            Audience = configuration["TokenAudience"]
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
