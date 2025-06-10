using TaskManagement.MobileApp.Services;
using TaskManagement.MobileApp.ViewModels;

namespace TaskManagement.MobileApp.Views.Pages;

/**
 * todo Look into: https://learn.microsoft.com/en-us/dotnet/architecture/maui/mvvm
 * for commands; seems more idiomatic.. than the onclicks here.
 */
public partial class MainPage : ContentPage
{
    public MainPage(TaskService taskService, OfficeService officeService, LinkedObjectService linkedObjectService)
    {
        InitializeComponent();
        var viewModel = new MainPageViewModel(taskService, officeService, linkedObjectService);
        BindingContext = viewModel;
    }
}