using System.ComponentModel;
using System.Runtime.CompilerServices;
using TaskManagement.MobileApp.Models;

namespace TaskManagement.MobileApp.ViewModels;

// public class TaskCardViewModel : INotifyPropertyChanged
// {
//     private readonly ISubtitleResolverService _subtitleResolverService;
//     private string _subtitle;
//     public TaskCardModel Model { get; }
//
//     public event PropertyChangedEventHandler PropertyChanged;
//
//     public TaskCardViewModel(TaskCardModel model, ISubtitleResolverService subtitleResolverService)
//     {
//         Model = model;
//         _subtitleResolverService = subtitleResolverService;
//         _ = LoadSubtitleAsync();
//     }
//
//     public string Subtitle
//     {
//         get => _subtitle;
//         private set
//         {
//             if (_subtitle != value)
//             {
//                 _subtitle = value;
//                 OnPropertyChanged();
//             }
//         }
//     }
//
//     private async Task LoadSubtitleAsync()
//     {
//         Subtitle = await _subtitleResolverService.ResolveSubtitleAsync(Model.LinkedObjectId);
//     }
//
//     protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
//         => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
// }