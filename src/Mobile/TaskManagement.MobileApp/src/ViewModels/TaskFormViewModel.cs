using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using TaskManagement.MobileApp.Models;
using TaskManagement.MobileApp.Models.Collections;
using TaskManagement.MobileApp.Services;

namespace TaskManagement.MobileApp.ViewModels;

public partial class TaskFormViewModel : ObservableObject
{
    [ObservableProperty]
    private string _title = string.Empty;

    [ObservableProperty]
    private string _description = string.Empty;

    [ObservableProperty]
    private DateTime _dueDate = DateTime.Today.AddDays(7); // Default deadline 1 week from today

    [ObservableProperty]
    private bool _isNewTask = true;

    [ObservableProperty]
    private object? _assignedUser;

    [ObservableProperty]
    private object? _linkedObject;

    [ObservableProperty]
    private bool _isBusy;

    public ObservableCollection<UserItem> Colleagues { get; } = [];
    public ObservableCollection<LinkedObjectItem> ManagedObjects { get; } = [];
    
    public TaskFormViewModel(UserTask? task = null)
    {
        if (task is null) return;
        
        IsNewTask = false;
        Title = task.Title;
        Description = task.Description ?? string.Empty;
        DueDate = task.DueDate;
        AssignedUser = Colleagues.FirstOrDefault(u => u.Id == task.UserId);
        LinkedObject = ManagedObjects.FirstOrDefault(o => o.Id == task.LinkedObjectId);
    }

    // Commands
    [RelayCommand]
    private async Task SaveTaskAsync()
    {
        if (IsBusy)
            return;

        if (!ValidateForm())
            return;

        try
        {
            IsBusy = true;

            // TODO: Implement save logic
            // Example:
            // var task = new TaskItem
            // {
            //     Title = Title,
            //     Description = Description,
            //     Deadline = Deadline,
            //     UserId = (SelectedUser as User)?.Id,
            //     ObjectId = IsNewTask ? (SelectedObject as TaskObject)?.Id : null
            // };
            // 
            // if (IsNewTask)
            //     await _taskService.CreateTaskAsync(task);
            // else
            //     await _taskService.UpdateTaskAsync(task);

            // Navigate back or show success message
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            // Handle error - show alert or log
            await Shell.Current.DisplayAlert("Error", $"Failed to save task: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task CancelAsync()
    {
        // Navigate back without saving
        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    private async Task LoadDataAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;

            // TODO: Load users and objects from your data service
            // Example:
            // var users = await _userService.GetUsersAsync();
            // var objects = await _objectService.GetObjectsAsync();
            // 
            // Users.Clear();
            // foreach (var user in users)
            //     Users.Add(user);
            // 
            // Objects.Clear();
            // foreach (var obj in objects)
            //     Objects.Add(obj);

            // For demo purposes, add some sample data:
            Colleagues.Clear();
            Colleagues.Add(new UserItem(new Guid(), "John", "Doe"));
            Colleagues.Add(new UserItem(new Guid(), "Jane", "Smith"));
            Colleagues.Add(new UserItem(new Guid(), "Bob", "Johnson"));

            ManagedObjects.Clear();
            ManagedObjects.Add(new LinkedObjectItem(new Guid(), LinkedObjectType.Relation, "Henk"));
            ManagedObjects.Add(new LinkedObjectItem(new Guid(), LinkedObjectType.DamageClaim, "Auto"));
            ManagedObjects.Add(new LinkedObjectItem(new Guid(), LinkedObjectType.InsurancePolicy, "Rolis Polis"));
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", $"Failed to load data: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
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

        if (DueDate >= DateTime.Today) return true;
        
        Shell.Current.DisplayAlert("Validation Error", "Deadline kan niet in het verleden liggen.", "OK");
        return false;

    }
}