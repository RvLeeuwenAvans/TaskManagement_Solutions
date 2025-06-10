using TaskManagement.MobileApp.Services.Authentication;
using TaskManagement.MobileApp.ViewModels;

namespace TaskManagement.MobileApp.Views.Pages;

public partial class LoginPage : ContentPage
{
    public LoginPage(AuthService authService)
    {
        InitializeComponent();
        BindingContext = new LoginViewModel(authService);
    }
}