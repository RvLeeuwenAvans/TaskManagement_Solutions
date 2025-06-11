using TaskManagement.Client.Clients;
using TaskManagement.MobileApp.Services.Repositories.Interfaces;

namespace TaskManagement.MobileApp.Services.Authentication;

public class AuthRepository(UserAuthenticationClient client): IAuthRepository
{
    public Task<string> LoginAsync(string email, string password)
    {
        return client.AuthenticateUserAsync(email, password);
    }

    public void Logout()
    {
        client.Logout();
    }
}