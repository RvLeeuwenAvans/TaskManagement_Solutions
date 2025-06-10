using TaskManagement.MobileApp.Services;
using TaskManagement.MobileApp.ViewModels;

namespace TaskManagement.MobileApp.Views.Pages;

public partial class MainPage : ContentPage
{
    public MainPage(TaskService taskService, OfficeService officeService, LinkedObjectService linkedObjectService)
    {
        InitializeComponent();
        var viewModel = new MainPageViewModel(taskService, officeService, linkedObjectService);
        BindingContext = viewModel;
    }
}