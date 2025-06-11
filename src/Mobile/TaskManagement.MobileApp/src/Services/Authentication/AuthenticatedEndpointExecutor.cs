using System.Net;
using TaskManagement.MobileApp.Services.Repositories.Interfaces;

namespace TaskManagement.MobileApp.Services.Authentication;

public class AuthenticatedEndpointExecutor(IAuthRepository authRepository)
{
    private IAuthRepository AuthRepository { get; } = authRepository;

    public async Task<T> Execute<T>(Func<Task<T>> action)
    {
        try
        {
            return await action();
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
        {
            AuthRepository.Logout();
            await Shell.Current.GoToAsync("//Login");
        }

        throw new Exception("Unexpected flow during logout");
    }

    public async Task Execute(Func<Task> action)
    {
        try
        {
            await action();
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
        {
            AuthRepository.Logout();
            await Shell.Current.GoToAsync("//Login");
        }
    }
}