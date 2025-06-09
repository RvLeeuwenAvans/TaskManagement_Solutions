namespace TaskManagement.MobileApp.Views;

public partial class TaskFormView : ContentView
{
    public TaskFormView()
    {
        InitializeComponent();
    }

    private void OnContainerSizeChanged(object sender, EventArgs e)
    {
        // this code only exists because winui3 is dumb and uses other sizing logic for the datepicker element.
        // if they ever make this properly work with maui; delete this please.
        if (sender is Grid container && DeadlinePicker != null)
        {
            #if WINDOWS
                DeadlinePicker.WidthRequest = container.Width;
            #endif
        }
    }
}