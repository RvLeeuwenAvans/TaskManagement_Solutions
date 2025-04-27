using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TaskManagement.Domain.Office.User;

namespace TaskManagement.Application.Services;

public class TokenService(UserService userService, IConfiguration configuration)
{
    private readonly string _key =
        configuration["Jwt:Key"] ?? throw new InvalidOperationException("Secret misconfigured");

    private readonly string _issuer =
        configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("Issuer misconfigured");

    private readonly string _audience =
        configuration["Jwt:Audience"] ?? throw new InvalidOperationException("Audience misconfigured");

    private readonly int _expiryMinutes = int.Parse(configuration["Jwt:TokenExpiryMinutes"] ?? "60");

    public async Task<string?> AuthenticateUserAsync(string email, string password)
    {
        var user = await userService.GetUserByEmail(email);

        return user is null ? null :
            userService.VerifyPassword(user, user.Password, password) ? GenerateToken(user) :
            null;
    }

    private string? GenerateToken(User user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
            new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.FirstName),
            new Claim(ClaimTypes.Role, "User")
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
        var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_expiryMinutes),
            signingCredentials: signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}