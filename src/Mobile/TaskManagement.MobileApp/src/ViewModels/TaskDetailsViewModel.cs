using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TaskManagement.MobileApp.Models.Collections;
using TaskManagement.MobileApp.Services;
using ViewState = TaskManagement.MobileApp.Helpers.Enums.ViewState;

namespace TaskManagement.MobileApp.ViewModels;

public partial class TaskDetailsViewModel(
    TaskService taskService,
    LinkedObjectService linkedObjectService)
    : ObservableObject, IQueryAttributable
{
    private readonly TaskService _taskService = taskService ?? throw new ArgumentNullException(nameof(taskService));

    private readonly LinkedObjectService _linkedObjectService =
        linkedObjectService ?? throw new ArgumentNullException(nameof(linkedObjectService));

    private Guid _taskId;

    [ObservableProperty] private ViewState _currentState = ViewState.Loading;
    [ObservableProperty] private string? _taskTitle;
    [ObservableProperty] private string? _description;
    [ObservableProperty] private DateTime? _dueDate;
    [ObservableProperty] private LinkedObjectItem? _linkedObject;
    [ObservableProperty] private ObservableCollection<NoteItemViewModel> _notes = [];

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("taskId", out var value))
        {
            var taskIdString = value.ToString();
            if (Guid.TryParse(taskIdString, out var taskId))
            {
                _taskId = taskId;
                _ = Task.Run(InitializeAsync);
            }
            else
            {
                CurrentState = ViewState.Error;
            }
        }
        else
        {
            CurrentState = ViewState.Error;
        }
    }

    private async void InitializeAsync()
    {
        try
        {
            CurrentState = ViewState.Loading;

            var task = await _taskService.GetTaskByIdAsync(_taskId);

            TaskTitle = task.Title;
            Description = task.Description ?? "No description provided.";
            DueDate = task.DueDate;
            if (task.LinkedObject != null)
                LinkedObject = await _linkedObjectService.GetLinkedObjectByResponse(task.LinkedObject);

            await LoadNotesAsync();

            CurrentState = ViewState.Success;
        }
        catch (Exception)
        {
            CurrentState = ViewState.Error;
        }
    }

    private async Task LoadNotesAsync()
    {
        try
        {
            var notes = await _taskService.GetNotesByTaskIdAsync(_taskId);

            Notes.Clear();
            foreach (var note in notes)
            {
                Notes.Add(new NoteItemViewModel
                {
                    Id = note.Id,
                    Content = note.Content,
                    CreatedAt = note.CreatedAt,
                });
            }
        }
        catch
        {
            await Shell.Current.DisplayAlert("Fout", "Notities konden niet worden geladen.", "OK");
            Notes.Clear();
        }
    }

    [RelayCommand]
    private async Task GoBack()
    {
        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    private async Task NavigateToUpdateTask()
    {
        await Shell.Current.GoToAsync($"task/edit?taskId={_taskId}");
    }

    [RelayCommand]
    private async Task AddNote()
    {
        var result = await Shell.Current.DisplayPromptAsync(
            "Nieuwe Notitie",
            "Voer de notitie in:",
            "Toevoegen",
            "Annuleren",
            placeholder: "Notitie tekst...");

        if (!string.IsNullOrWhiteSpace(result))
        {
            try
            {
                var success = await _taskService.CreateNoteAsync(_taskId, result);

                if (success)
                {
                    await LoadNotesAsync();
                }
                else
                {
                    await Shell.Current.DisplayAlert("Fout", "Notitie kon niet worden toegevoegd.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fout", $"Fout bij toevoegen notitie: {ex.Message}", "OK");
            }
        }
    }

    [RelayCommand]
    private async Task DeleteNote(NoteItemViewModel note)
    {
        try
        {
            var confirmed = await Shell.Current.DisplayAlert("Bevestigen",
                "Weet je zeker dat je deze notitie wilt verwijderen?", "Ja", "Nee");

            if (confirmed)
            {
                var success = await _taskService.DeleteNoteIdAsync(note.Id);

                if (success)
                {
                    Notes.Remove(note);
                }
                else
                {
                    await Shell.Current.DisplayAlert("Fout", "Notitie kon niet worden verwijderd.", "OK");
                }
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Fout", $"Fout bij verwijderen notitie: {ex.Message}", "OK");
        }
    }
}

// could probably move this into a seperate file; but it's pretty tightly coupled to the details page, and small enough.
// that i find this clearer.
public partial class NoteItemViewModel : ObservableObject
{
    [ObservableProperty] private Guid _id;
    [ObservableProperty] private string _content = string.Empty;
    [ObservableProperty] private DateTime _createdAt = DateTime.Now;
}