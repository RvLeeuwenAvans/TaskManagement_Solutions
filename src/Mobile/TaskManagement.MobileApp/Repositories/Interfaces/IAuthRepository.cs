namespace TaskManagement.MobileApp.Repositories.Interfaces;

public interface IAuthRepository
{
    Task<string> LoginAsync(string username, string password);
}