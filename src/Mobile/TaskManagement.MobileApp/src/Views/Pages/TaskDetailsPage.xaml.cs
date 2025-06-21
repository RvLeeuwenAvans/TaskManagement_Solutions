using TaskManagement.MobileApp.Models.Interfaces;
using TaskManagement.MobileApp.Services;
using TaskManagement.MobileApp.ViewModels;

namespace TaskManagement.MobileApp.Views.Pages;

public partial class TaskDetailsPage : ContentPage
{
    public TaskDetailsPage(TaskService taskService, LinkedObjectService linkedObjectService)
    {

        InitializeComponent();
        BindingContext = new TaskDetailsViewModel(taskService, linkedObjectService);
    }
}