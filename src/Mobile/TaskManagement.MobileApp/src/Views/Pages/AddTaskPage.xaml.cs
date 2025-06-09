using TaskManagement.MobileApp.Models.Interfaces;
using TaskManagement.MobileApp.Services;
using TaskManagement.MobileApp.ViewModels;

namespace TaskManagement.MobileApp.Views.Pages;

public partial class AddTaskPage : ContentPage
{
    public AddTaskPage(TaskService taskService, LinkedObjectService linkedObjectService, OfficeService officeService,
        IUserContext userContext)
    {
        InitializeComponent();
        var viewModel = new AddTaskViewModel(taskService, linkedObjectService, officeService, userContext);
        BindingContext = viewModel;
    }
}