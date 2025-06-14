using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace TaskManagement.MobileApp.ViewModels;

public partial class TaskDetailsViewModel : ObservableObject
{
    [ObservableProperty]
    private string _taskTitle = "Foto toevoegen";

    [ObservableProperty]
    private string _deadlineText = "Deadline: 15-7-2024";

    [ObservableProperty]
    private string _description = "Voeg sub foto's toe voor de inbedel op deze relatie 👍";

    [ObservableProperty]
    private string _relationName = "Henk van de takkenboom";

    [ObservableProperty]
    private ObservableCollection<NoteItemViewModel> _notes = [];
    
    public static void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("taskId", out var value))
        {
            var taskId = int.Parse(value.ToString() ?? string.Empty);
            // Load task details
        }
    }

    public TaskDetailsViewModel()
    {
        LoadSampleData();
    }

    private void LoadSampleData()
    {
        Notes.Add(new NoteItemViewModel
        {
            Id = 1,
            Content = "Relatie zal foto's nog mailen",
            CanDelete = true
        });
    }

    [RelayCommand]
    private async Task GoBack()
    {
        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    private async Task EditTask()
    {
        // Navigate to edit task page or show edit dialog
        await Shell.Current.DisplayAlert("Edit", "Edit task functionality", "OK");
    }

    [RelayCommand]
    private async Task AddNote()
    {
        string result = await Shell.Current.DisplayPromptAsync(
            "Nieuwe Notitie", 
            "Voer de notitie in:", 
            "Toevoegen", 
            "Annuleren", 
            placeholder: "Notitie tekst...");

        if (!string.IsNullOrWhiteSpace(result))
        {
            var newNote = new NoteItemViewModel
            {
                Id = Notes.Count + 1,
                Content = result,
                CanDelete = true
            };

            Notes.Add(newNote);
        }
    }

    [RelayCommand]
    private void DeleteNote(NoteItemViewModel note)
    {
        if (Notes.Contains(note))
        {
            Notes.Remove(note);
        }
    }
}

public partial class NoteItemViewModel : ObservableObject
{
    [ObservableProperty]
    private int _id;

    [ObservableProperty]
    private string _content = string.Empty;

    [ObservableProperty]
    private bool _canDelete = true;

    [ObservableProperty]
    private DateTime _createdAt = DateTime.Now;
}