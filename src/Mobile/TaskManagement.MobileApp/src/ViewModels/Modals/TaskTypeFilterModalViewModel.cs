using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using TaskManagement.MobileApp.ViewModels.messages;

namespace TaskManagement.MobileApp.ViewModels.Modals;

public partial class TaskTypeFilterModalViewModel(TaskTypeFilter currentFilter) : ObservableObject
{
    [ObservableProperty]
    private TaskTypeFilter _selectedType = currentFilter;

    [RelayCommand]
    private async Task Confirm()
    {
        await Shell.Current.Navigation.PopModalAsync(false);
        WeakReferenceMessenger.Default.Send(new TypeFilterSelectedMessage(SelectedType));
    }

    [RelayCommand]
    private async Task Cancel()
    {
        await Shell.Current.Navigation.PopModalAsync(false);
    }
}