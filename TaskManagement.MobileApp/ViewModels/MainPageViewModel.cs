using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TaskManagement.MobileApp.Models;
using TaskManagement.MobileApp.Services;

namespace TaskManagement.MobileApp.ViewModels;

public sealed class MainPageViewModel : INotifyPropertyChanged
{
    private ObservableCollection<TaskCardModel> AllTasks { get; set; } = [];
    public ObservableCollection<TaskCardModel> FilteredTaskCards { get; set; } = [];

    private readonly TaskService _taskService;

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    public MainPageViewModel(TaskService taskService)
    {
        _taskService = taskService;
        LoadTasks();
    }

    private void LoadTasks()
    {
        var userTasks = _taskService.GetUserTasks().Result;
        AllTasks = new ObservableCollection<TaskCardModel>(userTasks);
        FilteredTaskCards = new ObservableCollection<TaskCardModel>(AllTasks);
        OnPropertyChanged(nameof(FilteredTaskCards));
    }

    public void FilterTasks(string filter)
    {
        var filtered = filter switch
        {
            "Today" => AllTasks.Where(t => t.DueDate.Date == DateTime.Today),
            "Week" => AllTasks.Where(t =>
                t.DueDate.Date >= DateTime.Today &&
                t.DueDate.Date <= DateTime.Today.AddDays(7)
            ),
            _ => AllTasks
        };

        FilteredTaskCards.Clear();
        foreach (var task in filtered)
            FilteredTaskCards.Add(task);

        OnPropertyChanged(nameof(FilteredTaskCards));
    }
}