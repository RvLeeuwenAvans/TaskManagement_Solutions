using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using TaskManagement.MobileApp.Models.Interfaces;
using TaskManagement.MobileApp.Services;
using TaskManagement.MobileApp.ViewModels.messages;

namespace TaskManagement.MobileApp.ViewModels
{
    public partial class AddTaskViewModel : ObservableObject
    {
        [ObservableProperty] private TaskFormViewModel _formViewModel;

        public AddTaskViewModel(TaskService taskService, LinkedObjectService linkedObjectService,
            OfficeService officeService, IUserContext userContext)
        {
            FormViewModel = new TaskFormViewModel(taskService, linkedObjectService, officeService, userContext);
        }

        [RelayCommand]
        private async Task CreateTaskAsync()
        {
            var success = await FormViewModel.SaveTaskAsync();
            
            WeakReferenceMessenger.Default.Send(new TaskAddedMessage(true));
            
            if (success)
            {
                await Shell.Current.Navigation.PopAsync();
            }
        }
    }
}