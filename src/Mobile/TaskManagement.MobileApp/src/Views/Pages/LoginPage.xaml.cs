using TaskManagement.MobileApp.Services.Authentication;
using TaskManagement.MobileApp.ViewModels;

namespace TaskManagement.MobileApp.Views.Pages;

public partial class LoginPage : ContentPage
{
    public LoginPage(AuthenticationService authenticationService)
    {
        InitializeComponent();
        BindingContext = new LoginViewModel(authenticationService);
    }
}