using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TaskManagement.MobileApp.Models;
using TaskManagement.MobileApp.Services;

namespace TaskManagement.MobileApp.ViewModels;

/** todo look into: https://learn.microsoft.com/en-us/dotnet/communitytoolkit/maui/layouts/statecontainer **/
public sealed class MainPageViewModel : INotifyPropertyChanged
{
    private readonly TaskService _taskService;
    private ObservableCollection<TaskCardModel> _allTasks = [];
    private ObservableCollection<TaskCardModel> _filteredTaskCards = [];

    private Views.ViewState _currentState;

    public ObservableCollection<TaskCardModel> FilteredTaskCards
    {
        get => _filteredTaskCards;
        private set
        {
            _filteredTaskCards = value;
            OnPropertyChanged();
        }
    }

    public Views.ViewState CurrentState
    {
        get => _currentState;
        private set
        {
            _currentState = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    public MainPageViewModel(TaskService taskService)
    {
        _taskService = taskService;
        InitializeAsync();
    }

    private async void InitializeAsync()
    {
        try
        {
            CurrentState = Views.ViewState.Loading;
            await LoadTasksAsync();
        }
        catch (Exception)
        {
            CurrentState = Views.ViewState.Error;
        }
    }

    private async Task LoadTasksAsync()
    {
        var userTasks = await _taskService.GetUserTasks();
        _allTasks = new ObservableCollection<TaskCardModel>(userTasks);

        if (_allTasks.Count == 0)
        {
            CurrentState = Views.ViewState.Empty;
            FilteredTaskCards = [];
        }
        else
        {
            CurrentState = Views.ViewState.Success;
            FilteredTaskCards = new ObservableCollection<TaskCardModel>(_allTasks);
        }
    }

    public void FilterTasks(string filter)
    {
        if (CurrentState != Views.ViewState.Success)
            return; 

        var filtered = filter switch
        {
            "Today" => _allTasks.Where(t => t.DueDate.Date == DateTime.Today),
            "Week" => _allTasks.Where(t =>
                t.DueDate.Date >= DateTime.Today &&
                t.DueDate.Date <= DateTime.Today.AddDays(7)
            ),
            _ => _allTasks
        };

        FilteredTaskCards = new ObservableCollection<TaskCardModel>(filtered);
    }
}
