using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TaskManagement.MobileApp.Models;
using TaskManagement.MobileApp.Services;

namespace TaskManagement.MobileApp.ViewModels;

public enum TaskFilter
{
    All,
    Today,
    Week
}

public partial class MainPageViewModel : ObservableObject
{
    private readonly TaskService _taskService;
    private readonly LinkedObjectService _linkedObjectService;

    private ObservableCollection<TaskCardViewModel> _allTasks = [];

    [ObservableProperty] private ObservableCollection<TaskCardViewModel> _filteredTaskCards = [];
    [ObservableProperty] private Views.ViewState _currentState;
    [ObservableProperty] private TaskFilter _selectedFilter = TaskFilter.All;

    public MainPageViewModel(TaskService taskService, LinkedObjectService linkedObjectService)
    {
        _taskService = taskService ?? throw new ArgumentNullException(nameof(taskService));
        _linkedObjectService = linkedObjectService ?? throw new ArgumentNullException(nameof(linkedObjectService));

        InitializeAsync();
    }

    private async void InitializeAsync()
    {
        try
        {
            CurrentState = Views.ViewState.Loading;
            await LoadTasksAsync();
        }
        catch
        {
            CurrentState = Views.ViewState.Error;
        }
    }

    private async Task LoadTasksAsync()
    {
        var userTasks = await _taskService.GetUserTasks();
        var taskCardViewModels = new List<TaskCardViewModel>();

        foreach (var model in userTasks)
        {
            LinkedObjectModel? linkedObject = null;
            if (model.LinkedObjectResponse != null)
            {
                linkedObject = await _linkedObjectService.GetLinkedObjectByResponse(model.LinkedObjectResponse);
            }

            taskCardViewModels.Add(new TaskCardViewModel(model, _taskService, linkedObject, RemoveTask));
        }

        _allTasks = new ObservableCollection<TaskCardViewModel>(taskCardViewModels);

        if (_allTasks.Count == 0)
        {
            CurrentState = Views.ViewState.Empty;
            FilteredTaskCards = [];
        }
        else
        {
            CurrentState = Views.ViewState.Success;
            ApplyFilter(SelectedFilter);
        }
    }
    
    [RelayCommand]
    private async Task NavigateToAddTask()
    {
        await Shell.Current.GoToAsync("AddTaskPage");
    }

    [RelayCommand]
    private void Filter(string filter)
    {
        if (!Enum.TryParse<TaskFilter>(filter, out var parsedFilter)) return;
        SelectedFilter = parsedFilter;
        ApplyFilter(parsedFilter);
    }

    partial void OnSelectedFilterChanged(TaskFilter value)
    {
        ApplyFilter(value);
    }

    /**
    * The tabs on the main page are not actually tabs but filters; but to mock tab behavior, we show/hide the underline.
    */
    private void ApplyFilter(TaskFilter filter)
    {
        if (CurrentState != Views.ViewState.Success)
            return;

        var filtered = filter switch
        {
            TaskFilter.Today => _allTasks.Where(t => t.DueDate.Date == DateTime.Today),
            TaskFilter.Week => _allTasks.Where(t =>
                t.DueDate.Date >= DateTime.Today &&
                t.DueDate.Date <= DateTime.Today.AddDays(7)),
            _ => _allTasks
        };

        FilteredTaskCards = new ObservableCollection<TaskCardViewModel>(filtered);
    }

    private void RemoveTask(Guid taskId)
    {
        foreach (var collection in new[] { _allTasks, FilteredTaskCards })
        {
            var task = collection.FirstOrDefault(t => t.Id == taskId);
            if (task != null)
                collection.Remove(task);
        }
    }
}