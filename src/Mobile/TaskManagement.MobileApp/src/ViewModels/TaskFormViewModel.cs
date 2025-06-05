using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using TaskManagement.DTO.Office.User.Task;
using TaskManagement.MobileApp.Models;
using TaskManagement.MobileApp.Models.Collections;
using TaskManagement.MobileApp.Models.Interfaces;
using TaskManagement.MobileApp.Services;

namespace TaskManagement.MobileApp.ViewModels;

public partial class TaskFormViewModel : ObservableObject
{
    [ObservableProperty] private string _title = string.Empty;
    [ObservableProperty] private string? _description = string.Empty;
    [ObservableProperty] private DateTime _dueDate = DateTime.Today.AddDays(7);
    [ObservableProperty] private Views.ViewState _currentState = Views.ViewState.Loading;
    [ObservableProperty] private bool _isNewTask = true;
    [ObservableProperty] private UserItem? _assignedUser;
    [ObservableProperty] private LinkedObjectItem? _linkedObject;
    [ObservableProperty] private ObservableCollection<UserItem> _colleagues = [];
    [ObservableProperty] private ObservableCollection<LinkedObjectItem> _managedObjects = [];

    private readonly TaskService _taskService;
    private readonly LinkedObjectService _linkedObjectService;
    private readonly OfficeService _officeService;

    private readonly IUserContext _userContext;
    private readonly UserTaskResponse? _existingTask;

    public TaskFormViewModel(TaskService taskService, LinkedObjectService linkedObjectService,
        OfficeService officeService,
        IUserContext userContext,
        UserTaskResponse? task = null)
    {
        _taskService = taskService;
        _linkedObjectService = linkedObjectService;
        _officeService = officeService;
        _userContext = userContext;
        _existingTask = task;

        if (task is not null)
        {
            IsNewTask = false;
            Title = task.Title;
            Description = task.Description;
            DueDate = task.DueDate;
        }

        InitializeAsync();
    }

    public async Task<bool> SaveTaskAsync()
    {
        if (!ValidateForm())
            return false;

        try
        {
            CurrentState = Views.ViewState.Loading;

            var task = new UserTask(
                Title,
                Description,
                DueDate,
                AssignedUser!, // Validated in ValidateForm
                LinkedObject
            );

            if (_existingTask == null)
            {
                var currentUser = Colleagues.First(user => user.Id == _userContext.UserId);
                await _taskService.CreateTaskAsync(task, currentUser);
            }
            else
            {
                await _taskService.UpdateTaskAsync(task, _existingTask.Id);
            }

            CurrentState = Views.ViewState.Success;
            return true;
        }
        catch (Exception ex)
        {
            CurrentState = Views.ViewState.Error;
            await Shell.Current.DisplayAlert("Error", $"Failed to save task: {ex.Message}", "OK");
            return false;
        }
    }

    private bool ValidateForm()
    {
        if (string.IsNullOrWhiteSpace(Title))
        {
            Shell.Current.DisplayAlert("Validation Error", "Titel is verplicht.", "OK");
            return false;
        }

        if (AssignedUser == null)
        {
            Shell.Current.DisplayAlert("Validation Error", "Selecteer een gebruiker.", "OK");
            return false;
        }

        if (DueDate < DateTime.Today)
        {
            Shell.Current.DisplayAlert("Validation Error", "Deadline kan niet in het verleden liggen.", "OK");
            return false;
        }

        return true;
    }

    private async void InitializeAsync()
    {
        try
        {
            CurrentState = Views.ViewState.Loading;
            await PopulateFormCollections();

            AssignedUser = Colleagues.FirstOrDefault(u => u.Id == _userContext.UserId);

            if (_existingTask is not null)
            {
                AssignedUser = Colleagues.FirstOrDefault(u => u.Id == _existingTask.User.Id);

                if (_existingTask.LinkedObject != null)
                {
                    LinkedObject = await _linkedObjectService.GetLinkedObjectByResponse(_existingTask.LinkedObject);
                }
            }

            CurrentState = Views.ViewState.Success;
        }
        catch
        {
            CurrentState = Views.ViewState.Error;
        }
    }

    private async Task PopulateFormCollections()
    {
        try
        {
            var newColleagues = new ObservableCollection<UserItem>(
                await _officeService.GetUsersByOfficeAsync()
            );

            var newManagedObjects = new ObservableCollection<LinkedObjectItem>(
                await _officeService.GetManagedObjectsByOffice()
            );

            Colleagues = newColleagues;
            ManagedObjects = newManagedObjects;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to populate form collections", ex);
        }
    }
}