using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using TaskManagement.Client.Clients;
using TaskManagement.DTO.Office.User;

namespace TaskManagement.CMS.Services.Authentication;

public class AuthenticationService(UserAuthenticationClient authenticationClient)
{
    private Timer? _tokenExpirationTimer;

    public bool IsAuthenticated { get; private set; }
    public string? CurrentUserEmail { get; private set; }

    public event Action? AuthenticationStateChanged;

    public async Task<bool> LoginAsync(string email, string password)
    {
        try
        {
            var token = await authenticationClient.AuthenticateUserAsync(email, password);
            var (userRole, expirationTime) = ParseJwtToken(token);
            // Check if the user is Admin
            if (userRole != nameof(UserRole.Admin))
            {
                throw new Exception("Only administrators are allowed access to the CMS.");
            }
            // Logout when token expires
            SetupTokenExpirationTimer(expirationTime);
            IsAuthenticated = true;
            CurrentUserEmail = email;
            
            AuthenticationStateChanged?.Invoke();
            
            return true;
        }
        catch (Exception)
        {
            await ClearExpirationLogout();
            return false;
        }
    }

    public async Task LogoutAsync()
    {
        try
        {
            // unset token from clients
            authenticationClient.Logout();
        }
        finally
        {
            await ClearExpirationLogout();
        }
    }
    
    public Task<bool> EnsureAuthenticatedAsync()
    {
        return Task.FromResult(IsAuthenticated);
    }

    /// <summary>
    /// Executes an API call and handles automatic logout on unauthorized responses
    /// </summary>
    public async Task<T> ExecuteAuthenticatedRequest<T>(Func<Task<T>> action)
    {
        try
        {
            return await action();
        }
        catch (HttpRequestException ex) when (IsUnauthorized(ex))
        {
            await ClearExpirationLogout();
            throw new UnauthorizedAccessException("Session expired. Please log in again.");
        }
        catch (Exception ex) when (IsUnauthorizedMessage(ex))
        {
            await ClearExpirationLogout();
            throw new UnauthorizedAccessException("Session expired. Please log in again.");
        }
    }

    /// <summary>
    /// Executes an API call and handles automatic logout on unauthorized responses
    /// </summary>
    public async Task ExecuteAuthenticatedRequest(Func<Task> action)
    {
        try
        {
            await action();
        }
        catch (HttpRequestException ex) when (IsUnauthorized(ex))
        {
            await ClearExpirationLogout();
            throw new UnauthorizedAccessException("Token is invalid.");
        }
        catch (Exception ex) when (IsUnauthorizedMessage(ex))
        {
            await ClearExpirationLogout();
            throw new UnauthorizedAccessException("Token is invalid.");
        }
    }
    
    private static (string role, DateTime expiration) ParseJwtToken(string jwtToken)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(jwtToken);

        var userIdClaim = jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub);
        if (userIdClaim == null)
            throw new Exception("UserId not found in token");

        var roleClaim = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
        if (roleClaim == null)
            throw new Exception("Role not found in token");

        // Get token expiration
        var expiration = jwt.ValidTo;
        if (expiration <= DateTime.UtcNow)
            throw new Exception("Token has expired");

        return (roleClaim.Value, expiration);
    }

    private void SetupTokenExpirationTimer(DateTime expirationTime)
    {
        _tokenExpirationTimer?.Dispose();

        var timeUntilExpiration = expirationTime.Subtract(DateTime.UtcNow);

        // If the token expires in less than 1 minute, logout immediately
        if (timeUntilExpiration.TotalMinutes < 1)
        {
            _ = Task.Run(async () => await ClearExpirationLogout());
            return;
        }

        // Set the timer to logout 30 seconds before actual expiration
        var timerDuration = timeUntilExpiration.Subtract(TimeSpan.FromSeconds(30));

        _tokenExpirationTimer = new Timer(async void (_) =>
            {
                try
                {
                    await ClearExpirationLogout();
                }
                catch (Exception)
                {
                    // ignored
                }
            }, null, timerDuration,
            Timeout.InfiniteTimeSpan);
    }
    
    private async Task ClearExpirationLogout()
    {
        if (_tokenExpirationTimer != null) await _tokenExpirationTimer.DisposeAsync();
        _tokenExpirationTimer = null;
        IsAuthenticated = false;
        CurrentUserEmail = null;

        AuthenticationStateChanged?.Invoke();
        await Task.CompletedTask;
    }    
    
    private static bool IsUnauthorized(HttpRequestException ex) =>
        ex.Data["StatusCode"]?.ToString() == nameof(HttpStatusCode.Unauthorized);

    private static bool IsUnauthorizedMessage(Exception ex) =>
        ex.Message.Contains("401") || ex.Message.Contains("Unauthorized");
}