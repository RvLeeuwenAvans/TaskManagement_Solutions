using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TaskManagement.MobileApp.Services.Authentication;
using ViewState = TaskManagement.MobileApp.Helpers.Enums.ViewState;

namespace TaskManagement.MobileApp.ViewModels;

public partial class LoginViewModel(AuthService authService) : ObservableObject
{
    [ObservableProperty] private string _email = string.Empty;
    [ObservableProperty] private string _password = string.Empty;
    [ObservableProperty] private string _errorMessage = string.Empty;
    [ObservableProperty] private ViewState _currentState = ViewState.Success;

    [RelayCommand]
    private async Task LoginAsync()
    {
        ErrorMessage = string.Empty;

        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
        {
            ErrorMessage = "Vul zowel e-mail als wachtwoord in.";
            CurrentState = ViewState.Error;
            return;
        }

        CurrentState = ViewState.Loading;
        try
        {
            var success = await authService.AuthenticateUser(Email, Password);
            if (success)
            {
                    await Shell.Current.GoToAsync("//OverviewPage");
            }
            else
            {
                ErrorMessage = "Inloggen mislukt. Controleer uw gegevens.";
                CurrentState = ViewState.Error;
            }
        }
        catch(Exception ex)
        {
            ErrorMessage = ex.Message;
            CurrentState = ViewState.Error;
        }
        finally
        {
            if (string.IsNullOrEmpty(ErrorMessage))
                CurrentState = ViewState.Success;
        }
    }
}