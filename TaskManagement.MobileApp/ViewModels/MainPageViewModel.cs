using System.Collections.ObjectModel;

namespace TaskManagement.MobileApp.ViewModels;

public class MainPageViewModel
{
    public ObservableCollection<TaskItem> TaskItems { get; set; }

    public MainViewModel()
    {
        TaskItems = new ObservableCollection<TaskItem>
        {
            new TaskItem { IconLetter = "H", Title = "Foto toevoegen", Subtitle = "Relatie: Henk van den Rooijboom", DateText = "16-7-2023" },
            new TaskItem { IconLetter = "K", Title = "Rapport uploaden", Subtitle = "Relatie: Kim Vermeer", DateText = "17-7-2023" },
            // Add more tasks
        };
    }
}
