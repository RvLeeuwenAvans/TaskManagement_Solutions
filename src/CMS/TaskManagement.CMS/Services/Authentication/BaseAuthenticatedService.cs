namespace TaskManagement.CMS.Services.Authentication;

public abstract class BaseAuthenticatedService(AuthenticationService authenticationService)
{
    protected async Task<T> ExecuteIfAuthenticatedAsync<T>(Func<Task<T>> action)
    {
        if (!await authenticationService.EnsureAuthenticatedAsync())
            throw new UnauthorizedAccessException("User must be authenticated to access this resource.");

        return await authenticationService.ExecuteAuthenticatedRequest(action);
    }

    protected async Task ExecuteIfAuthenticatedAsync(Func<Task> action)
    {
        if (!await authenticationService.EnsureAuthenticatedAsync())
            throw new UnauthorizedAccessException("User must be authenticated to access this resource.");

        await authenticationService.ExecuteAuthenticatedRequest(action);
    }
}