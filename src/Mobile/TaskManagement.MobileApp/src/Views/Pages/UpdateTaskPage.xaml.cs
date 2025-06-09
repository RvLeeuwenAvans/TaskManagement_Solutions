using TaskManagement.MobileApp.Models.Interfaces;
using TaskManagement.MobileApp.Services;
using TaskManagement.MobileApp.ViewModels;

namespace TaskManagement.MobileApp.Views.Pages;

public partial class UpdateTaskPage : ContentPage
{
    public UpdateTaskPage(TaskService taskService, LinkedObjectService linkedObjectService,
        OfficeService officeService, IUserContext userContext)
    {
        InitializeComponent();
        var viewModel = new UpdateTaskViewModel(taskService, linkedObjectService,
            officeService, userContext);
        BindingContext = viewModel;
    }
}