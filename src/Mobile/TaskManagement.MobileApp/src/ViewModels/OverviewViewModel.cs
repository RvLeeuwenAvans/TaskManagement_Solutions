using System.Collections.ObjectModel;
using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using TaskManagement.MobileApp.Models.Collections;
using TaskManagement.MobileApp.Services;
using TaskManagement.MobileApp.ViewModels.messages;
using TaskManagement.MobileApp.ViewModels.Modals;
using TaskManagement.MobileApp.Views.Modals;
using ViewState = TaskManagement.MobileApp.Helpers.Enums.ViewState;

namespace TaskManagement.MobileApp.ViewModels;

public partial class OverviewViewModel : ObservableObject
{
    private readonly TaskService _taskService;
    private readonly OfficeService _officeService;
    private readonly LinkedObjectService _linkedObjectService;

    private ObservableCollection<TaskCardViewModel> _allTasks = [];

    [ObservableProperty] private string _officeName = string.Empty;
    [ObservableProperty] private ObservableCollection<TaskCardViewModel> _filteredTaskCards = [];
    [ObservableProperty] private ViewState _currentState;

    [ObservableProperty] private TaskTypeFilter _selectedTaskTypeFilter = TaskTypeFilter.All;
    [ObservableProperty] private TaskDateRangeFilter _selectedDateRangeDateRangeFilter = TaskDateRangeFilter.All;

    public OverviewViewModel(TaskService taskService, OfficeService officeService,
        LinkedObjectService linkedObjectService)
    {
        _taskService = taskService ?? throw new ArgumentNullException(nameof(taskService));
        _officeService = officeService ?? throw new ArgumentNullException(nameof(officeService));
        _linkedObjectService = linkedObjectService ?? throw new ArgumentNullException(nameof(linkedObjectService));

        WeakReferenceMessenger.Default.Register<TaskAddedMessage>(this, void (_, _) => { InitializeAsync(); });
        WeakReferenceMessenger.Default.Register<TaskEditedMessage>(this, void (_, _) => { InitializeAsync(); });
        WeakReferenceMessenger.Default.Register<TaskClosedMessage>(this, void (_, _) => { InitializeAsync(); });
        WeakReferenceMessenger.Default.Register<UserAuthenticatedMessage>(this, void (_, _) => { InitializeAsync(); });
        WeakReferenceMessenger.Default.Register<TypeFilterSelectedMessage>(this,
            (_, filter) => { FilterByTaskTypeCommand.Execute(filter.Value); });

        InitializeAsync();
    }

    private async void InitializeAsync()
    {
        try
        {
            CurrentState = ViewState.Loading;
            await LoadTasksAsync();
            var office = await _officeService.GetCurrentUserOfficeAsync();
            OfficeName = office.Name;
        }
        catch
        {
            CurrentState = ViewState.Error;
        }
    }

    private async Task LoadTasksAsync()
    {
        var userTasks = await _taskService.GetCurrentUserTasksAsync();
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
            CurrentState = ViewState.Empty;
            FilteredTaskCards = [];
        }
        else
        {
            CurrentState = ViewState.Success;
            ApplyFilters();
        }
    }

    [RelayCommand]
    private static async Task NavigateToAddTask()
    {
        await Shell.Current.GoToAsync("task/add");
    }

    [RelayCommand]
    private void FilterByDateRange(string filter)
    {
        if (!Enum.TryParse<TaskDateRangeFilter>(filter, out var parsedFilter)) return;
        SelectedDateRangeDateRangeFilter = parsedFilter;
    }

    [RelayCommand]
    private void FilterByTaskType(TaskTypeFilter type)
    {
        SelectedTaskTypeFilter = type;
    }

    [RelayCommand]
    private async Task ShowTaskTypeModalAsync()
    {
        var modal = new TaskTypeFilterModal(new TaskTypeFilterModalViewModel(SelectedTaskTypeFilter));
        await Shell.Current.Navigation.PushModalAsync(modal);
    }

    partial void OnSelectedTaskTypeFilterChanged(TaskTypeFilter value)
    {
        if (!Enum.IsDefined(typeof(TaskTypeFilter), value))
            throw new InvalidEnumArgumentException(nameof(value), (int)value, typeof(TaskTypeFilter));

        ApplyFilters();
    }

    partial void OnSelectedDateRangeDateRangeFilterChanged(TaskDateRangeFilter value)
    {
        if (!Enum.IsDefined(typeof(TaskDateRangeFilter), value))
            throw new InvalidEnumArgumentException(nameof(value), (int)value, typeof(TaskDateRangeFilter));

        ApplyFilters();
    }

    private void ApplyFilters()
    {
        if (CurrentState is not (ViewState.Success or ViewState.Empty))
            return;

        CurrentState = ViewState.Loading;

        IEnumerable<TaskCardViewModel> unfiltered = _allTasks;

        var filtered = ApplyDateRangeFilter(unfiltered, SelectedDateRangeDateRangeFilter);
        filtered = ApplyTaskTypeFilter(filtered, SelectedTaskTypeFilter);

        FilteredTaskCards = new ObservableCollection<TaskCardViewModel>(filtered);
        CurrentState = FilteredTaskCards.Count == 0
            ? ViewState.Empty
            : ViewState.Success;
    }

    private static IEnumerable<TaskCardViewModel> ApplyDateRangeFilter(IEnumerable<TaskCardViewModel> tasks,
        TaskDateRangeFilter dateRangeFilter)
    {
        return dateRangeFilter switch
        {
            TaskDateRangeFilter.Today => tasks.Where(t => t.DueDate.Date == DateTime.Today),
            TaskDateRangeFilter.Week => tasks.Where(t =>
                t.DueDate.Date >= DateTime.Today &&
                t.DueDate.Date <= DateTime.Today.AddDays(7)),
            _ => tasks
        };
    }

    private static IEnumerable<TaskCardViewModel> ApplyTaskTypeFilter(IEnumerable<TaskCardViewModel> tasks,
        TaskTypeFilter typeFilter)
    {
        return typeFilter switch
        {
            TaskTypeFilter.Relation => tasks.Where(t => t.LinkedObjectType == LinkedObjectType.Relation),
            TaskTypeFilter.DamageClaim => tasks.Where(t => t.LinkedObjectType == LinkedObjectType.DamageClaim),
            TaskTypeFilter.InsurancePolicy =>
                tasks.Where(t => t.LinkedObjectType == LinkedObjectType.InsurancePolicy),
            TaskTypeFilter.None => tasks.Where(t => t.LinkedObjectType == null),
            _ => tasks
        };
    }
}