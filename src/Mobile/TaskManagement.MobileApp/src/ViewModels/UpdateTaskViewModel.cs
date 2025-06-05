using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TaskManagement.MobileApp.Models.Interfaces;
using TaskManagement.MobileApp.Services;

namespace TaskManagement.MobileApp.ViewModels;

public partial class UpdateTaskViewModel(
    TaskService taskService,
    LinkedObjectService linkedObjectService,
    OfficeService officeService,
    IUserContext userContext)
    : ObservableObject, IQueryAttributable
{
    private readonly TaskService _taskService = taskService ?? throw new ArgumentNullException(nameof(taskService));

    private readonly LinkedObjectService _linkedObjectService =
        linkedObjectService ?? throw new ArgumentNullException(nameof(linkedObjectService));

    private Guid _taskId;

    [ObservableProperty] private TaskFormViewModel? _formViewModel;
    [ObservableProperty] private Views.ViewState _currentState = Views.ViewState.Loading;


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
                CurrentState = Views.ViewState.Error;
            }
        }
        else
        {
            CurrentState = Views.ViewState.Error;
        }
    }

    private async void InitializeAsync()
    {
        try
        {
            CurrentState = Views.ViewState.Loading;

            var existingTask = await _taskService.GetTaskByIdAsync(_taskId);

            FormViewModel = new TaskFormViewModel(_taskService, _linkedObjectService,
                officeService, userContext, existingTask);

            CurrentState = Views.ViewState.Success;
        }
        catch
        {
            CurrentState = Views.ViewState.Error;
        }
    }

    [RelayCommand]
    private async Task UpdateTaskAsync()
    {
        if (FormViewModel == null) return;

        var success = await FormViewModel.SaveTaskAsync();
        if (success)
        {
            await Shell.Current.Navigation.PopAsync();
        }
    }

    [RelayCommand]
    private async Task DeleteTaskAsync()
    {
        var confirmed = await Shell.Current.DisplayAlert("Bevestigen",
            "Weet je zeker dat je deze taak wilt verwijderen?", "Ja", "Nee");

        if (confirmed)
        {
            try
            {
                var success = await _taskService.CloseTaskAsync(_taskId);
                if (success)
                {
                    await Shell.Current.Navigation.PopAsync();
                }
                else
                {
                    await Shell.Current.DisplayAlert("Fout", "Taak kon niet worden verwijderd.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fout", $"Fout bij verwijderen: {ex.Message}", "OK");
            }
        }
    }
}