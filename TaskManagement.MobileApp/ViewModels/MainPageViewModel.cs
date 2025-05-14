using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TaskManagement.DTO.Office.User;
using TaskManagement.DTO.Office.User.Task;
using TaskManagement.MobileApp.Models;
using TaskManagement.MobileApp.Services.Helpers.Builders;

namespace TaskManagement.MobileApp.ViewModels;

public sealed class MainPageViewModel : INotifyPropertyChanged
{
    private ObservableCollection<TaskCardModel> AllTasks { get; set; }
    public ObservableCollection<TaskCardModel> FilteredTaskCards { get; set; }

    public MainPageViewModel()
    {
        // Todo: get from API.
        var userTasks = new List<UserTaskResponseDto>
        {
            new()
            {
                User =
                    new UserResponseDto
                    {
                        FirstName = "Henk",
                        LastName = "de Tank",
                        Email = "Tank@gmail.com"
                    },
                Title = "Foto toevoegen",
                Description = "Relatie: Henk van den Rooijboom",
                DueDate = new DateTime(2025, 7, 16),
                Id = Guid.Empty,
                CreatorName = "Henk"
            },
            new()
            {
                User = new UserResponseDto
                {
                    FirstName = "Lisa",
                    LastName = "uit Pisa",
                    Email = "Pisa@Toren.com"
                },
                Title = "Contract ondertekenen",
                Description = "Relatie: Lisa de Jong",
                DueDate = DateTime.Today,
                Id = Guid.Empty,
                CreatorName = "Lisa"
            },
            new()
            {
                User = new UserResponseDto
                {
                    FirstName = "Mark",
                    LastName = "uit Hark",
                    Email = "Hark@nl.nl"
                },
                Title = "Belafspraak maken",
                Description = "Relatie: Mark Jansen",
                DueDate = DateTime.Today.AddDays(3),
                Id = Guid.Empty,
                CreatorName = "Mark"
            }
        };

        AllTasks = new ObservableCollection<TaskCardModel>(
            userTasks.Select(task => TaskCardModelBuilder.From(task).BuildTaskCard())
        );

        FilteredTaskCards = new ObservableCollection<TaskCardModel>(AllTasks);
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

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}