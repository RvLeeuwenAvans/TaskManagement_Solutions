using TaskManagement.MobileApp.ViewModels;

namespace TaskManagement.MobileApp.Views.Pages;

public partial class TaskDetailsPage : ContentPage
{
    public TaskDetailsPage()
    {
        InitializeComponent();
    }

    public TaskDetailsPage(TaskDetailsViewModel viewModel) : this()
    {
        BindingContext = viewModel;
    }
}