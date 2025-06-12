using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using TaskManagement.MobileApp.Services.Authentication;
using TaskManagement.MobileApp.ViewModels.messages;
using ViewState = TaskManagement.MobileApp.Helpers.Enums.ViewState;

namespace TaskManagement.MobileApp.ViewModels;

public partial class LoginViewModel(AuthenticationService authenticationService) : ObservableObject
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
            var success = await authenticationService.AuthenticateUser(Email, Password);
            if (success)
            {
                WeakReferenceMessenger.Default.Send(new UserAuthenticatedMessage(true));
                await Shell.Current.GoToAsync("///OverviewPage");
            }
            else
            {
                ErrorMessage = "Inloggen mislukt. Controleer uw gegevens.";
                CurrentState = ViewState.Error;
            }
        }
        catch (Exception ex)
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