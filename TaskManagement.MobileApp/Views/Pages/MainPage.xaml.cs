using TaskManagement.MobileApp.Services;
using TaskManagement.MobileApp.ViewModels;

namespace TaskManagement.MobileApp.Views.Pages;

/**
 * Look into: https://learn.microsoft.com/en-us/dotnet/architecture/maui/mvvm
 * for commands; seems more idiomatic.. than the onclicks here.
 */
public partial class MainPage : ContentPage
{
    private readonly MainPageViewModel _viewModel;

    public MainPage(TaskService taskService)
    {
        InitializeComponent();
        _viewModel = new MainPageViewModel(taskService);
        BindingContext = _viewModel;
    }

    /**
    * The tabs on the main page ore not actually tabs but filters; but to mock tab behavior, we show/hide the underline.
    */
    private void OnTabClicked(object sender, EventArgs e)
    {
        // Reset all underlines
        AllUnderline.IsVisible = false;
        TodayUnderline.IsVisible = false;
        WeekUnderline.IsVisible = false;

        string filter = sender switch
        {
            Button { Text: "Alle" } => ShowUnderline(AllUnderline, "All"),
            Button { Text: "Vandaag" } => ShowUnderline(TodayUnderline, "Today"),
            Button { Text: "Week" } => ShowUnderline(WeekUnderline, "Week"),
            _ => "All"
        };

        _viewModel.FilterTasks(filter);
    }

    private static string ShowUnderline(BoxView underline, string filter)
    {
        underline.IsVisible = true;
        return filter;
    }
}