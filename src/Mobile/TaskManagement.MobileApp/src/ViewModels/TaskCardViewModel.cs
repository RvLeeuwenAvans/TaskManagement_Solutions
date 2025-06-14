using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Messaging;
using TaskManagement.MobileApp.Models;
using TaskManagement.MobileApp.Models.Collections;
using TaskManagement.MobileApp.Services;
using TaskManagement.MobileApp.ViewModels.messages;

namespace TaskManagement.MobileApp.ViewModels;

public partial class TaskCardViewModel(
    UserTaskCardItem model,
    TaskService taskService,
    LinkedObjectItem? linkedObject = null)
    : ObservableObject
{
    private UserTaskCardItem Model { get; } = model;

    public char CreatorInitial => Model.CreatorInitial;

    public Guid Id => Model.Id;
    public string Title => Model.Title;
    public DateTime DueDate => Model.DueDate;
    public string? Subtitle => linkedObject?.Name;
    public LinkedObjectType? LinkedObjectType => linkedObject?.Type;

    [RelayCommand]
    private async Task CloseTaskAsync()
    {
        if (await taskService.CloseTaskAsync(Model.Id))
        {
            WeakReferenceMessenger.Default.Send(new TaskAddedMessage(true));
            await Shell.Current.Navigation.PopAsync();
        }
    }

    [RelayCommand]
    private async Task NavigateToUpdateTask()
    {
        await Shell.Current.GoToAsync($"UpdateTaskPage?taskId={Model.Id}");
    }
    
    
    [RelayCommand]
    private async Task NavigateToTaskDetails()
    {
        await Shell.Current.GoToAsync($"TaskDetailsPage?taskId={Model.Id}");
    }
}