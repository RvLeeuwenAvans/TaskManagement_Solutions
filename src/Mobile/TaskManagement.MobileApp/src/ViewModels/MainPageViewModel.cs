using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using TaskManagement.MobileApp.Models.Collections;
using TaskManagement.MobileApp.Services;
using TaskManagement.MobileApp.ViewModels.messages;

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

        WeakReferenceMessenger.Default.Register<TaskAddedMessage>(this, async void (_, _) => { InitializeAsync(); });
        WeakReferenceMessenger.Default.Register<TaskEditedMessage>(this, async void (_, _) => { InitializeAsync(); });
        WeakReferenceMessenger.Default.Register<TaskClosedMessage>(this, async void (_, _) => { InitializeAsync(); });

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
        var userTasks = await _taskService.GetUserTasksAsync();
        var taskCardViewModels = new List<TaskCardViewModel>();

        foreach (var model in userTasks)
        {
            LinkedObjectItem? linkedObject = null;
            if (model.LinkedObjectResponse != null)
            {
                linkedObject = await _linkedObjectService.GetLinkedObjectByResponse(model.LinkedObjectResponse);
            }

            taskCardViewModels.Add(new TaskCardViewModel(model, _taskService, linkedObject));
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
    private static async Task NavigateToAddTask()
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
}