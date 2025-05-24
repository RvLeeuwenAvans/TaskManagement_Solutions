namespace TaskManagement.MobileApp.Models;

public record TaskCardModel(
    char CreatorInitial,
    string Title,
    Guid? LinkedObjectId,
    DateTime DueDate);