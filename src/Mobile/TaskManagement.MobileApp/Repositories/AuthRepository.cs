using TaskManagement.Client.Clients;
using TaskManagement.MobileApp.Repositories.Interfaces;

namespace TaskManagement.MobileApp.Repositories;

public class AuthRepository(UserAuthenticationClient client): IAuthRepository
{
    public Task<string> LoginAsync(string email, string password)
    {
       return client.AuthenticateUserAsync(email, password);
    }
}