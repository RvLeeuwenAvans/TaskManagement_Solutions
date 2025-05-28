using TaskManagement.DTO.Office.User.Task.LinkedObject;

namespace TaskManagement.MobileApp.Models;

public record TaskCardModel(
    char CreatorInitial,
    string Title,
    LinkedObjectResponse? LinkedObjectResponse,
    DateTime DueDate);