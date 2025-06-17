using TaskManagement.MobileApp.Views.Pages;

namespace TaskManagement.MobileApp;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        
        Routing.RegisterRoute("task/add", typeof(AddTaskPage));
        Routing.RegisterRoute("task/edit", typeof(UpdateTaskPage));
        Routing.RegisterRoute("task/details", typeof(TaskDetailsPage));
    }
}