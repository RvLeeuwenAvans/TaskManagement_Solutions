using TaskManagement.MobileApp.Services.Repositories.Interfaces;

namespace TaskManagement.MobileApp.Services.Authentication.Utils;

public class SessionManager(IAuthRepository authRepository)
{
    public async Task InvalidateSessionAsync()
    {
        authRepository.Logout();
        await Shell.Current.GoToAsync("///LoginPage");
    }
}