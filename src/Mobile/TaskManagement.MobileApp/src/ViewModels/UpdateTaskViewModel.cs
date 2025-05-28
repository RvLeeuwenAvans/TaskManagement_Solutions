using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace TaskManagement.MobileApp.ViewModels
{
    public class UpdateTaskViewModel : INotifyPropertyChanged
    {
        private string _taskId;

        public TaskFormViewModel FormViewModel { get; }

        public ICommand BackCommand { get; }
        public ICommand UpdateTaskCommand { get; }
        public ICommand DeleteTaskCommand { get; }

        public UpdateTaskViewModel()
        {
            FormViewModel = new TaskFormViewModel();
            BackCommand = new Command(async () => await GoBack());
            UpdateTaskCommand = new Command(async () => await UpdateTask(), CanUpdateTask);
            DeleteTaskCommand = new Command(async () => await DeleteTask());
        }

        public UpdateTaskViewModel(string taskId) : this()
        {
            _taskId = taskId;
            LoadTask(taskId);
        }

        private async void LoadTask(string taskId)
        {
            try
            {
                // TODO: Load existing task data from your service/repository
                // var existingTask = await _taskService.GetTaskByIdAsync(taskId);
                // 
                // FormViewModel.Title = existingTask.Title;
                // FormViewModel.Description = existingTask.Description;
                // FormViewModel.Deadline = existingTask.Deadline;
                // FormViewModel.SelectedUser = existingTask.AssignedUser;
                // FormViewModel.SelectedObject = existingTask.RelatedObject;

                // For now, load sample data
                FormViewModel.Title = "Foto toevoegen";
                FormViewModel.Description = "Voeg sub foto's toe voor de inboedel op deze relatie 📸";
                FormViewModel.Deadline = new DateTime(2023, 7, 15);
                FormViewModel.SelectedUser = new UserModel { Name = "Ik", Initials = "Ik", AvatarColor = "#4CAF50" };
                FormViewModel.SelectedObject = new ObjectModel { Name = "Schade: Water", Color = "#F44336" };
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Fout", $"Kan taak niet laden: {ex.Message}", "OK");
            }
        }

        private async Task GoBack()
        {
            // Navigate back to previous page
            await Shell.Current.GoToAsync("..");
        }

        private async Task UpdateTask()
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

                // TODO: Update the actual task using your service/repository
                // var updatedTask = new TaskModel 
                // {
                //     Id = _taskId,
                //     Title = FormViewModel.Title,
                //     Description = FormViewModel.Description,
                //     Deadline = FormViewModel.Deadline,
                //     AssignedUser = FormViewModel.SelectedUser,
                //     RelatedObject = FormViewModel.SelectedObject
                // };
                // 
                // await _taskService.UpdateTaskAsync(updatedTask);

                // Show success message
                await Application.Current.MainPage.DisplayAlert("Succes", "Taak succesvol bijgewerkt!", "OK");

                // Navigate back to main page
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Fout", $"Er is een fout opgetreden: {ex.Message}", "OK");
            }
        }

        private async Task DeleteTask()
        {
            try
            {
                bool confirm = await Application.Current.MainPage.DisplayAlert(
                    "Bevestigen", 
                    "Weet je zeker dat je deze taak wilt verwijderen?", 
                    "Ja", "Nee");

                if (!confirm) return;

                // TODO: Delete the actual task using your service/repository
                // await _taskService.DeleteTaskAsync(_taskId);

                // Show success message
                await Application.Current.MainPage.DisplayAlert("Succes", "Taak succesvol verwijderd!", "OK");

                // Navigate back to main page
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Fout", $"Er is een fout opgetreden: {ex.Message}", "OK");
            }
        }

        private bool CanUpdateTask()
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