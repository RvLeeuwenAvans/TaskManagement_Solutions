using System.IdentityModel.Tokens.Jwt;
using TaskManagement.MobileApp.Models.Interfaces;
using TaskManagement.MobileApp.Services.Repositories.Interfaces;

namespace TaskManagement.MobileApp.Services.Authentication;

public class AuthService(IUserContext userContext, IAuthRepository authRepository)
{
    /**
     * Will return true if the user is authenticated, false otherwise.
     */
    public async Task<bool> AuthenticateUser(string email, string password)
    {
        var jwtToken = await authRepository.LoginAsync(email, password);

        try
        {
            await SetUserContextFromJwt(jwtToken);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
    
    private Task SetUserContextFromJwt(string jwtToken)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(jwtToken);

        var userIdClaim = jwt.Claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Sub);

        if (userIdClaim == null)
            throw new Exception("UserId not found");

        userContext.UserId = Guid.Parse(userIdClaim.Value);
        return Task.CompletedTask;
    }
}