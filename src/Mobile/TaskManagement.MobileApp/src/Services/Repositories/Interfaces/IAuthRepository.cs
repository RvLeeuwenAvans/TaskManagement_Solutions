namespace TaskManagement.MobileApp.Services.Repositories.Interfaces;

public interface IAuthRepository
{
    Task<string> LoginAsync(string username, string password);
    void Logout();
}