using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using TaskManagement.MobileApp.Models;
using TaskManagement.MobileApp.Models.Collections;
using TaskManagement.MobileApp.Services;

namespace TaskManagement.MobileApp.ViewModels;

public partial class TaskCardViewModel : ObservableObject
{
    private UserTaskCardItem Model { get; }
    public ICommand CloseTaskCommand { get; }

    public char CreatorInitial => Model.CreatorInitial;

    public Guid Id => Model.Id;
    public string Title => Model.Title;
    public DateTime DueDate => Model.DueDate;
    public string? Subtitle => _linkedObject?.Name;
    public LinkedObjectType? LinkedObjectType => _linkedObject?.Type;

    private readonly TaskService _taskService;
    private readonly LinkedObjectItem? _linkedObject;
    private readonly Action<Guid>? _onTaskClosed;

    public TaskCardViewModel(UserTaskCardItem model, TaskService taskService, LinkedObjectItem? linkedObject = null,
        Action<Guid>? onTaskClosed = null)
    {
        Model = model;
        _taskService = taskService;
        _linkedObject = linkedObject;
        _onTaskClosed = onTaskClosed;
        CloseTaskCommand = new AsyncRelayCommand(CloseTaskAsync);
    }

    private async Task CloseTaskAsync()
    {
        if (await _taskService.CloseTaskAsync(Model.Id))
        {
            _onTaskClosed?.Invoke(Model.Id);
        }
    }

    [RelayCommand]
    private static async Task NavigateToUpdateTask()
    {
        await Shell.Current.GoToAsync("UpdateTaskPage");
    }
}