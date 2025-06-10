using TaskManagement.MobileApp.ViewModels.Modals;

namespace TaskManagement.MobileApp.Views.Modals;

public partial class TaskTypeFilterModal : ContentPage
{
    public TaskTypeFilterModal(TaskTypeFilterModalViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}