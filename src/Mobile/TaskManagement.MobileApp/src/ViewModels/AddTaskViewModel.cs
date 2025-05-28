using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace TaskManagement.MobileApp.ViewModels
{
    public class AddTaskViewModel : INotifyPropertyChanged
    {
        public TaskFormViewModel FormViewModel { get; }

        public ICommand BackCommand { get; }
        public ICommand CreateTaskCommand { get; }

        public AddTaskViewModel()
        {
            FormViewModel = new TaskFormViewModel();
            BackCommand = new Command(async () => await GoBack());
            CreateTaskCommand = new Command(async () => await CreateTask(), CanCreateTask);
        }

        private async Task GoBack()
        {
            // Navigate back to previous page
            await Shell.Current.GoToAsync("..");
        }

        private async Task CreateTask()
        {
            try
            {
                // Validate form
                if (string.IsNullOrWhiteSpace(FormViewModel.Title))
                {
                    await Application.Current.MainPage.DisplayAlert("Fout", "Titel is verplicht", "OK");
                    return;
                }

                if (string.IsNullOrWhiteSpace(FormViewModel.Description))
                {
                    await Application.Current.MainPage.DisplayAlert("Fout", "Omschrijving is verplicht", "OK");
                    return;
                }

                // TODO: Create the actual task using your service/repository
                // var newTask = new TaskModel 
                // {
                //     Title = FormViewModel.Title,
                //     Description = FormViewModel.Description,
                //     Deadline = FormViewModel.Deadline,
                //     AssignedUser = FormViewModel.SelectedUser,
                //     RelatedObject = FormViewModel.SelectedObject
                // };
                // 
                // await _taskService.CreateTaskAsync(newTask);

                // Show success message
                await Application.Current.MainPage.DisplayAlert("Succes", "Taak succesvol aangemaakt!", "OK");

                // Navigate back to main page
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Fout", $"Er is een fout opgetreden: {ex.Message}", "OK");
            }
        }

        private bool CanCreateTask()
        {
            return !string.IsNullOrWhiteSpace(FormViewModel?.Title) && 
                   !string.IsNullOrWhiteSpace(FormViewModel?.Description);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}