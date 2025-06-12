using TaskManagement.MobileApp.Views.Pages;

namespace TaskManagement.MobileApp;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        
        Routing.RegisterRoute("LoginPage", typeof(LoginPage));
        Routing.RegisterRoute("OverviewPage", typeof(OverviewPage));
        Routing.RegisterRoute("AddTaskPage", typeof(AddTaskPage));
        Routing.RegisterRoute("UpdateTaskPage", typeof(UpdateTaskPage));
    }
}