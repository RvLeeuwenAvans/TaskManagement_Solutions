namespace TaskManagement.MobileApp.Views.Pages;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }
    
    /**
     * The tabs on the main page ore not actually tabs but filters; but to mock tab behavior, we show/hide the underline.
     */
    private void OnTabClicked(object sender, EventArgs e)
    {
        AllUnderline.IsVisible = false;
        TodayUnderline.IsVisible = false;
        WeekUnderline.IsVisible = false;

        if (sender == AllButton)
            AllUnderline.IsVisible = true;
        else if (sender == TodayButton)
            TodayUnderline.IsVisible = true;
        else if (sender == WeekButton)
            WeekUnderline.IsVisible = true;
    }
}