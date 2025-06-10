using TaskManagement.MobileApp.Services;
using TaskManagement.MobileApp.ViewModels;

namespace TaskManagement.MobileApp.Views.Pages;

public partial class OverviewPage : ContentPage
{
    public OverviewPage(TaskService taskService, OfficeService officeService, LinkedObjectService linkedObjectService)
    {
        InitializeComponent();
        BindingContext =  new OverviewPageViewModel(taskService, officeService, linkedObjectService);;
    }
}