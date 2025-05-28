using CommunityToolkit.Mvvm.ComponentModel;
using TaskManagement.MobileApp.Models;

namespace TaskManagement.MobileApp.ViewModels;

public class TaskCardViewModel(TaskCardModel model, LinkedObjectModel? linkedObject = null) : ObservableObject
{
    private TaskCardModel Model { get; } = model;
    
    public char CreatorInitial => Model.CreatorInitial;
    public string Title => Model.Title;
    public DateTime DueDate => Model.DueDate;
    
    public string? Subtitle => linkedObject?.Name;
    public LinkedObjectType? LinkedObjectType => linkedObject?.Type;
}