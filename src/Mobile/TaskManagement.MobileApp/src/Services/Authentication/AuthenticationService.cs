using System.IdentityModel.Tokens.Jwt;
using TaskManagement.DTO.Office.User;
using TaskManagement.MobileApp.Models.Interfaces;
using TaskManagement.MobileApp.Services.Repositories.Interfaces;

namespace TaskManagement.MobileApp.Services.Authentication;

public class AuthenticationService(IUserContext userContext, IAuthRepository authRepository, IUserRepository userRepository)
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

    private async Task SetUserContextFromJwt(string jwtToken)
    {
        userContext.UserId = ParseJwtToken(jwtToken);
        var user = await userRepository.GetUserById(userContext.UserId);
        userContext.OfficeId = user.OfficeId;
    }
    
    private static Guid ParseJwtToken(string jwtToken)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(jwtToken);
        var userIdClaim = jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub);

        if (userIdClaim == null)
            throw new Exception("UserId not found");
        
        var roleClaim = jwt.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Role);
        
        if (roleClaim == null)
            throw new Exception("Role not found");
        
        if (roleClaim.Value == nameof(UserRole.Admin))
        {
            throw new Exception("Administrators are only allowed access to the CMS.");
        }

        return Guid.Parse(userIdClaim.Value);
    }
}