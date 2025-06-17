using System.Net;
using TaskManagement.MobileApp.Services.Authentication.Utils;

namespace TaskManagement.MobileApp.Services.Authentication;

public class AuthenticatedRequestExecutor(SessionManager sessionManager)
{
    
    /// <summary>
    /// Executes an asynchronous function that returns a result. 
    /// If the request is unauthorized, the session is invalidated.
    /// </summary>
    private SessionManager SessionManager { get; } = sessionManager;

    public async Task<T> Execute<T>(Func<Task<T>> action)
    {
        try
        {
            return await action();
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
        {
           await SessionManager.InvalidateSessionAsync();
        }
        throw new Exception("Unexpected flow during logout");
    }

    /// <summary>
    /// Executes an asynchronous action that returns void.
    /// If the request is unauthorized, the session is invalidated.
    /// </summary>
    public async Task Execute(Func<Task> action)
    {
        try
        {
            await action();
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
        {
            await SessionManager.InvalidateSessionAsync();
        }
    }
}