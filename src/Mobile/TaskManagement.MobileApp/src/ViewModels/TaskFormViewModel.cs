using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace TaskManagement.MobileApp.ViewModels
{
    public class TaskFormViewModel : INotifyPropertyChanged
    {
        private string _title = string.Empty;
        private string _description = string.Empty;
        private DateTime _deadline = DateTime.Today.AddDays(7);
        private UserModel _selectedUser;
        private ObjectModel _selectedObject;

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        public DateTime Deadline
        {
            get => _deadline;
            set => SetProperty(ref _deadline, value);
        }

        public UserModel SelectedUser
        {
            get => _selectedUser ?? new UserModel { Name = "Ik", Initials = "Ik", AvatarColor = "#4CAF50" };
            set => SetProperty(ref _selectedUser, value);
        }

        public ObjectModel SelectedObject
        {
            get => _selectedObject;
            set => SetProperty(ref _selectedObject, value);
        }

        public bool HasSelectedObject => SelectedObject != null;

        public ICommand SelectDateCommand { get; }
        public ICommand SelectUserCommand { get; }
        public ICommand SelectObjectCommand { get; }

        public TaskFormViewModel()
        {
            SelectDateCommand = new Command(async () => await SelectDate());
            SelectUserCommand = new Command(async () => await SelectUser());
            SelectObjectCommand = new Command(async () => await SelectObject());
        }

        private async Task SelectDate()
        {
            // This would open a date picker
            // For now, just set a sample date
            Deadline = DateTime.Today.AddDays(7);
        }

        private async Task SelectUser()
        {
            // This would open a user selection page/popup
            // For now, just toggle between "Ik" and "Collega Henk"
            if (SelectedUser?.Name == "Ik")
            {
                SelectedUser = new UserModel { Name = "Collega Henk", Initials = "H", AvatarColor = "#2196F3" };
            }
            else
            {
                SelectedUser = new UserModel { Name = "Ik", Initials = "Ik", AvatarColor = "#4CAF50" };
            }
        }

        private async Task SelectObject()
        {
            // This would open an object selection page/popup
            // For now, just toggle between null and "Schade: Water"
            if (SelectedObject == null)
            {
                SelectedObject = new ObjectModel { Name = "Schade: Water", Color = "#F44336" };
            }
            else
            {
                SelectedObject = null;
            }
            OnPropertyChanged(nameof(HasSelectedObject));
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

    public class UserModel
    {
        public string Name { get; set; }
        public string Initials { get; set; }
        public string AvatarColor { get; set; }
    }

    public class ObjectModel
    {
        public string Name { get; set; }
        public string Color { get; set; }
    }
}